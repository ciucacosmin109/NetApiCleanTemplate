using NetApiCleanTemplate.WebApi.Middlewares;
using NetApiCleanTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;
using NetApiCleanTemplate.WebApi.Swagger;
using NetApiCleanTemplate.WebApi.Logging;

// Builder =============================================================================================
var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.SetupSerilog();

// Configure services
NetApiCleanTemplate.Core.Registration.ConfigureServices(builder.Configuration, builder.Services);
NetApiCleanTemplate.Infrastructure.Registration.ConfigureServices(builder.Configuration, builder.Services);
NetApiCleanTemplate.WebApi.Registration.ConfigureServices(builder.Configuration, builder.Services);

// Configure kestrel
builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.AddServerHeader = false;
});

// Others
builder.Logging.AddConsole();

// Configure Infrastructure & WebApi ===================================================================
var app = builder.Build();
app.Logger.LogInformation("Creating WebApi...");

app.Logger.LogInformation("Seeding Database...");
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        // Migrate
        var catalogContext = scopedProvider.GetRequiredService<AppDbContext>();
        if (catalogContext.Database.IsSqlServer())
        {
            catalogContext.Database.Migrate();
        }

        // Migrate identity db 
        var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        var identityUserMan = scopedProvider.GetRequiredService<UserManager<AppUser>>();
        var identityRoleMan = scopedProvider.GetRequiredService<RoleManager<AppRole>>();
        if (identityContext.Database.IsSqlServer())
        {
            identityContext.Database.Migrate();
        }
        await AppIdentityDbContextSeed.SeedAsync(identityContext, identityUserMan, identityRoleMan);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Logger.LogInformation("Configuring WebApi...");

// Configure the HTTP request pipeline ================================================================

// Nginx headers
app.UseForwardedHeaders(new() { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
// app.UseHttpsRedirection(); // It is handled by NGINX

// Routing + Rate limiter + Security
app.UseRouting();
app.UseRateLimiter();
app.UseHstsMiddleware(app.Configuration);

// Swagger + Static files
app.UseSwaggerMiddleware(app.Configuration);
app.UseStaticFiles();

// Cors
app.UseCors();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Catch exceptions + Logging
app.UseMiddleware<ExceptionMiddleware>();
app.UseRequestLoggingMiddleware();

// Multitenancy + Database
app.UseMiddleware<MultitenancyMiddleware>();
app.UseMiddleware<DatabaseUpdaterMiddleware>();

// Language
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options!.Value);
app.UseLanguageMiddleware();

// Controllers
app.MapControllers()
    .RequireCors("FrontendPolicy")
    .RequireRateLimiting("RateLimiterPolicy");
app.Map("/", context => Task.Run(() => context.Response.Redirect("/swagger/index.html")));

// Run =================================================================================================
app.Logger.LogInformation("Starting WebApi..."); 
app.Run();

// Integration tests: alternative to the InternalsVisibleTo
public sealed partial class Program { }