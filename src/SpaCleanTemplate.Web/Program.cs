using SpaCleanTemplate.Core;
using SpaCleanTemplate.Infrastructure;
using SpaCleanTemplate.Web;

// Define the modules
var coreModule = ModuleFactory.Create(typeof(CoreModule));
var infrastructureModule = ModuleFactory.Create(typeof(InfrastructureModule), coreModule);

// The WebModule depends on all the other modules to be able to setup DI, etc
var theOtherModules = new List<Module> { coreModule, infrastructureModule };
var webModule = ModuleFactory.Create(typeof(InfrastructureModule), theOtherModules);

// Builder
var builder = WebApplication.CreateBuilder(args);

// Setup WebModule services (+theOtherModules)
webModule.ConfigureServices(builder.Configuration, builder.Services);

// Configure WebModule (+theOtherModules)
var app = builder.Build();
webModule.Configure(builder.Configuration, app, builder.Environment.IsDevelopment());

// Run
app.Run();
