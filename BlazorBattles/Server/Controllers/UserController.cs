﻿using BlazorBattles.Server.Data;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //Tkanks to authorize attribute it's possible to access User object (see action GetBananas).
    public class UserController : ControllerBase
    {
        private readonly DataContext dataContext;

        public UserController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User> GetUser() => await dataContext.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        
        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {
            var user = await GetUser();
            return Ok(user.Bananas);
        }

        [HttpPut("addbananas")]
        public async Task<IActionResult> AddBananas([FromBody]int bananas)
        {
            var user = await GetUser();
            user.Bananas += bananas;

            await dataContext.SaveChangesAsync();
            return Ok(user.Bananas);
        }
    }
}
