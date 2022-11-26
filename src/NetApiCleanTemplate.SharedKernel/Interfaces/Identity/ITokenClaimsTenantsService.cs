using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Identity;

public interface ITokenClaimsTenantsService
{
    Task<string> GetTokenAsync(string userName, string selectedTenant);
}

