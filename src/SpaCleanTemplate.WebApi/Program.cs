using SpaCleanTemplate.Core;
using SpaCleanTemplate.Infrastructure;
using SpaCleanTemplate.WebApi.Middlewares;
using SpaCleanTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SpaCleanTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

// Builder =============================================================================================
var builder = WebApplication.CreateBuilder(args);

// Configure services for Infrastructure & WebApi
SpaCleanTemplate.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
SpaCleanTemplate.WebApi.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

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
        var identityUserMan = scopedProvider.GetRequiredService<UserManager<IdentityUser>>();
        var identityRoleMan = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
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

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    // Use swagger
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwaggerUI(c => // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    }); 

    // Others
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Run =================================================================================================
app.Logger.LogInformation("Starting WebApi..."); 
app.Run();
