using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.SharedKernel.Exceptions;

namespace NetApiCleanTemplate.Web.Controllers.Demo;

[Route("api/[controller]")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;

    public DemoController(ILogger<DemoController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<string> GetWeatherForecast()
    {
        _logger.LogInformation("GetWeatherForecast");
        return new string[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }

    [HttpGet("TestDomainException")]
    public void TestDomainException()
    {
        _logger.LogInformation("TestDuplicateException");
        throw new DomainException("Oh, it's a duplicate :(");
    }

    [HttpGet("TestAuthentication")]
    [Authorize]
    public string TestAuthentication()
    {
        _logger.LogInformation("TestAuthentication");
        return "You have access here :)";
    }
}

