using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserUnitController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUtilityService utilityService;

        public UserUnitController(DataContext dataContext, IUtilityService utilityService)
        {
            this.context = dataContext;
            this.utilityService = utilityService;
        }

        [HttpPost]
        public async Task<IActionResult> BuildUserUnit([FromBody]int unitId)
        {
            var unit = await context.Units.FirstOrDefaultAsync<Unit>(u => u.Id == unitId);
            var user = await utilityService.GetUser();
            if (user.Bananas < unit.BananaCost)
            {
                return BadRequest("Not enough bananas!");
            }

            user.Bananas -= unit.BananaCost;

            var newUserUnit = new UserUnit
            {
                UnitId = unitId,
                UserId = user.Id,
                HitPoints = unit.HitPoints
            };
            context.UserUnits.Add(newUserUnit);
            await context.SaveChangesAsync();

            return Ok(newUserUnit); 
        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await utilityService.GetUser();
            var userUnits = await context.UserUnits.Where(uu => uu.UserId == user.Id).ToListAsync();

            var response = userUnits.Select(
                unit => new UserUnitResponse
                {
                    UnitId = unit.UnitId,
                    HitPoints = unit.HitPoints
                });

            return Ok(response);
        }

        [HttpPost("revive")]
        public async Task<IActionResult> ReviveArmy()
        {
            var user = await utilityService.GetUser();
            var userUnits = await context.UserUnits
                .Where(u => u.UserId == user.Id)
                .Include(u => u.Unit)
                .ToListAsync();

            int bananaCost = 1000;

            if (user.Bananas < 1000)
            {
                return BadRequest("You need to 1000 banans to revive your army!");
            }

            bool armyAlreadyAlive = true;
            foreach (var userUnit in userUnits)
            {
                if (userUnit.HitPoints <= 0)
                {
                    armyAlreadyAlive = false;
                    userUnit.HitPoints = new Random().Next(0, userUnit.Unit.HitPoints);
                }
            }

            if (armyAlreadyAlive)
                return Ok("Your army is already alive.");

            user.Bananas -= bananaCost;

            await context.SaveChangesAsync();

            return Ok("Army revived!");
        }
    }
}
