using NetApiCleanTemplate.WebApi.Middlewares;
using NetApiCleanTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

// Builder =============================================================================================
var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline when running in development
if (builder.Environment.IsDevelopment())
{
    // Use swagger
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwaggerUI(c => // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
    {
        // Core
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{NetApiCleanTemplate.WebApi.Registration.SwaggerName} v1");

        // Display
        c.DefaultModelExpandDepth(0);
        c.DefaultModelRendering(ModelRendering.Model);
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(DocExpansion.List);

        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.EnableFilter();
        c.ShowExtensions();

        // Other
        c.DocumentTitle = "NetApiCleanTemplate";
        //c.InjectJavascript("/swagger/multitenancy-auth.js");
    }); 

    // Others
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<MultitenancyMiddleware>();

app.MapControllers();

// Run =================================================================================================
app.Logger.LogInformation("Starting WebApi..."); 
app.Run();

