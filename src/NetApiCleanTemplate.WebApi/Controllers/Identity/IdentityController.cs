using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Infrastructure.Identity;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;
using NetApiCleanTemplate.WebApi.Controllers.Identity.Exceptions;
using NetApiCleanTemplate.WebApi.Controllers.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenClaimsTenantsService _tokenClaimsService;
    private readonly IConfiguration _configuration;
    private readonly IMultitenancyManager multitenancyManager;
    private readonly AppIdentityDbContext context;

    public IdentityController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        ITokenClaimsTenantsService tokenClaimsService,
        IConfiguration configuration,
        IMultitenancyManager multitenancyManager,
        AppIdentityDbContext context
    ) {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenClaimsService = tokenClaimsService;
        _configuration = configuration;
        this.multitenancyManager = multitenancyManager;
        this.context = context;
    }

    [HttpPost]
    [Route("Login")]
    [SwaggerOperation(
        Summary = "Authenticates a user",
        Description = "Authenticates a user",
        OperationId = "Identity.Login",
        Tags = new[] { "Identity" })
    ]
    public async Task<AuthenticateResponse> Login([FromBody] AuthenticationRequest request)
    {
        // TODO: Move the logic from the controller

        var response = new AuthenticateResponse(request.CorrelationId());
        if (request.Username == null)
        {
            return response;
        }

        // To enable password failures to trigger account lockout, set lockoutOnFailure: true 
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);
        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (!result.Succeeded)
        {
            return response;
        }

        // Check tenant
        var allTenants = await context.AppTenants
            .Select(x => x.TenantId)
            .ToListAsync();

        if (!String.IsNullOrWhiteSpace(request.TenantId) && !allTenants.Contains(request.TenantId))
        {
            response.Result = false;
            return response;
        }

        // Check if the user has permission to access this tenant
        var allowedTenants = await context.AppUserTenants
            .Where(x => x.User.UserName == request.Username)
            .Select(x => x.Tenant.TenantId)
            .ToListAsync();

        var user = await _userManager.FindByNameAsync(request.Username);
        if (!user.HasAccessToAllTenants && !allowedTenants.Contains(request.TenantId))
        {
            response.Result = false;
            return response;
        }

        // Ok
        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username, request.TenantId);
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
    public async Task<AuthenticateResponse> Register([FromBody] RegistrationRequest request)
    {
        var userExists = await _userManager.FindByNameAsync(request.Username);
        if (userExists != null)
        {
            throw new UserAlreadyExistsException();
        }

        var allTenants = await context.AppTenants
            .ToDictionaryAsync(x => x.TenantId, x => x.Id);
        request.TenantIds = request.TenantIds.Where(x => allTenants.ContainsKey(x)).ToArray();

        AppUser user = new() {
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username,
            AppUserTenants = request.TenantIds.Select(x =>
                new AppUserTenant {
                    TenantId = allTenants[x],
                }
            ).ToList()
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

    [HttpGet]
    [Route("GetClaims")]
    [Authorize]
    [SwaggerOperation(
        Summary = "",
        Description = "",
        OperationId = "Identity.GetClaims",
        Tags = new[] { "Identity" })
    ]
    public IEnumerable<Claim> GetClaims()
    {
        return HttpContext.User.Claims
            .Select(x => new Claim(x.Type, x.Value, x.ValueType, x.Issuer));
    }

    [HttpGet]
    [Route("GetTenant")]
    [Authorize]
    [SwaggerOperation(
        Summary = "",
        Description = "",
        OperationId = "Identity.GetTenant",
        Tags = new[] { "Identity" })
    ]
    public Tenant GetTenant()
    {
        return multitenancyManager.CurrentTenant;
    }
}

