using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BattleController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUtilityService utilityService;

        public BattleController(DataContext context, IUtilityService utilityService)
        {
            this.context = context;
            this.utilityService = utilityService;
        }

        [HttpPost]
        public async Task<IActionResult> StartBattle([FromBody] int opponentId)
        {
            var attacker = await utilityService.GetUser();
            var opponent = await context.Users.FindAsync(opponentId);
            if (opponent==null || opponent.IsDeleted)
            {
                return NotFound("Opponent not avaliable!");
            }

            return Ok();
        }
    }
}
