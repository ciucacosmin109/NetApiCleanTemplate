using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Guards;
using NetApiCleanTemplate.Core.General.Guards;
using NetApiCleanTemplate.Core.Services.DemoService;
using NetApiCleanTemplate.Core.Services.DemoService.Input;
using NetApiCleanTemplate.Core.Services.DemoService.Output;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Guards;

namespace NetApiCleanTemplate.Web.Controllers.Demo;

[Route("api/[controller]")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> logger;
    private readonly IDemoService demoService;

    public DemoController(
        ILogger<DemoController> logger,
        IDemoService demoService
    ) {
        this.logger = logger;
        this.demoService = demoService;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<string> GetWeatherForecast()
    {
        logger.LogInformation("GetWeatherForecast");
        return new string[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }

    [HttpGet("TestDomainException")]
    public void TestDomainException()
    {
        logger.LogInformation("TestDuplicateException");
        throw new DomainException("Oh, it's a duplicate :(");
    }

    [HttpGet("TestGuardClause")]
    public string TestGuardClause(string demoString)
    {
        logger.LogInformation("TestGuardClause");
        Guard.Against.InvalidDemoString(demoString);
        
        return demoString;
    }

    [HttpGet("TestAuthentication")]
    [Authorize]
    public string TestAuthentication()
    {
        logger.LogInformation("TestAuthentication");
        return "You have access here :)";
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<DemoDto>> GetAll()
    {
        return await demoService.GetAll();
    }

    [HttpGet("Create")]
    public async Task<int> Create(CreateDemoDto dto)
    {
        return await demoService.Create(dto);
    }

    [HttpGet("Update")]
    public async Task Update(UpdateDemoDto dto)
    {
        await demoService.Update(dto);
    }

    [HttpGet("Delete")]
    public async Task Delete(int id)
    {
        await demoService.Delete(id);
    }
}

