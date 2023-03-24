using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetApiCleanTemplate.Core.Constants;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

namespace NetApiCleanTemplate.Infrastructure.Identity.Services;

public class MultitenancyTokenClaimsService : ITokenClaimsService, ITokenClaimsTenantsService
{
    private readonly MultitenancyUserManager _userManager;

    public MultitenancyTokenClaimsService(
        MultitenancyUserManager userManager
    ) {
        _userManager = userManager;
    }

    public async Task<string> GetTokenAsync(string userName)
    {
        return await GetTokenAsync(userName, Tenant.DefaultTenantId);
    }

    public async Task<string> GetTokenAsync(string userName, string selectedTenant)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);

        var user = await _userManager.FindByNameAsync(userName);
        var claims = new List<Claim> { 
            new Claim(ClaimTypes.Name, userName),
            new Claim(CustomClaimTypes.SelectedTenant, selectedTenant)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Supported tenants
        /*var tenants = await _userManager.GetTenantsAsync(user);
        foreach (var tenant in tenants)
        {
            claims.Add(new Claim(CustomClaimTypes.SupportedTenant, tenant));
        }*/

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
