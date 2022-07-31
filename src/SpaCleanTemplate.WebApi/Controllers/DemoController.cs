using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaCleanTemplate.Core.Exceptions;

namespace SpaCleanTemplate.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("TestDuplicateException")]
        public void TestDuplicateException()
        {
            _logger.LogInformation("TestDuplicateException");
            throw new DuplicateException("Oh, it's a duplicate :(");
        }

        [HttpGet("TestAuthentication")]
        [Authorize]
        public string TestAuthentication()
        {
            _logger.LogInformation("TestAuthentication");
            return "You have access here :)";
        }
    }
}