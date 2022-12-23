using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Guards; 
using NetApiCleanTemplate.Core.Services.DemoService;
using NetApiCleanTemplate.Core.Services.DemoService.Input;
using NetApiCleanTemplate.Core.Services.DemoService.Output;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Guards;

namespace NetApiCleanTemplate.Web.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> logger;

    public AdminController(
        ILogger<AdminController> logger
    ) {
        this.logger = logger;
    }

    [HttpGet("TestAdminAuthorization")]
    public string TestAdminAuthorization()
    {
        logger.LogInformation("TestAdminAuthorization");
        return "You have access here :)";
    }
}

