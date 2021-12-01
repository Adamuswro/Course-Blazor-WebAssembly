using BlazorBattles.Server.Data;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public UtilityService(DataContext context, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<User> GetUser()
        {
            var userId = int.Parse(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }
    }
}
