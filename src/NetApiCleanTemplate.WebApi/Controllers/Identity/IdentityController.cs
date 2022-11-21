using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.SharedKernel.Interfaces;
using NetApiCleanTemplate.WebApi.Controllers.Identity.Exceptions;
using NetApiCleanTemplate.WebApi.Controllers.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly IConfiguration _configuration;

    public IdentityController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        ITokenClaimsService tokenClaimsService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenClaimsService = tokenClaimsService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Login")]
    [SwaggerOperation(
        Summary = "Authenticates a user",
        Description = "Authenticates a user",
        OperationId = "Identity.Login",
        Tags = new[] { "Identity" })
    ]
    public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticationRequest request)
    {
        var response = new AuthenticateResponse(request.CorrelationId());
        
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true 
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);
        if(request.Username == null)
        {
            return response;
        }

        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
        }
        return response;
    }

    [HttpPost]
    [Route("Register")]
    [SwaggerOperation(
        Summary = "Registers a user",
        Description = "Registers a user",
        OperationId = "Identity.Register",
        Tags = new[] { "Identity" })
    ]
    public async Task<ActionResult<AuthenticateResponse>> Register([FromBody] RegistrationRequest request)
    {
        var userExists = await _userManager.FindByNameAsync(request.Username);
        if (userExists != null)
        {
            throw new UserAlreadyExistsException();
        }

        IdentityUser user = new() {
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidUserDetailsException();
        }

        var response = new AuthenticateResponse(request.CorrelationId());
        response.Result = result.Succeeded;
        return response;
    }

}

