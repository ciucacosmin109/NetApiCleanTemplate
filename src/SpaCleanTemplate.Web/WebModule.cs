using SpaCleanTemplate.Core;

namespace SpaCleanTemplate.Web;

public class WebModule : Module
{
    public WebModule(ICollection<Module> dependencies) : base(dependencies) { }

    public override void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Configure the dependent modules
        base.ConfigureServices(configuration, services);

        // Add services to the container.
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

    }
    public override void Configure(IConfiguration configuration, IApplicationBuilder app, bool isDevEnv)
    {
        // Configure the dependent modules
        base.Configure(configuration, app, isDevEnv);

        if (app is not WebApplication webApp)
        {
            throw new ArgumentException("[app] must be a [WebApplication]");
        }

        // Configure the HTTP request pipeline.
        if (isDevEnv)
        {
            webApp.UseSwagger();
            webApp.UseSwaggerUI();
        }

        webApp.UseHttpsRedirection();

        webApp.UseAuthorization();

           
        webApp.MapControllers();

    }
}

