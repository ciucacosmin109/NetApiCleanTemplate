﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}

