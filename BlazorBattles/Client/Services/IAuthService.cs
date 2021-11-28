﻿using BlazorBattles.Client.Shared;
using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request); 
    }
}
