using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DataContext context;
        private readonly IUtilityService utilityService;

        public UserController(DataContext dataContext, IUtilityService utilityService)
        {
            this.context = dataContext;
            this.utilityService = utilityService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User> GetUser() => await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {
            var user = await utilityService.GetUser();
            return Ok(user.Bananas);
        }

        [HttpPut("addbananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await utilityService.GetUser();
            user.Bananas += bananas;

            await context.SaveChangesAsync();
            return Ok(user.Bananas);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await context.Users.Where(u => u.IsDeleted == false && u.IsConfirmed == true).ToListAsync();

            users = users.OrderByDescending(u => u.Victories)
                .ThenBy(u => u.Defeats)
                .ThenBy(u => u.DateCreated)
                .ToList();

            int rank = 1;
            var response = users.Select(u => new UserStatistics
            {
                Rank = rank++,
                UserId = u.Id,
                UserName = u.Username,
                Battles = u.Battles,
                Victories = u.Victories,
                Defeats = u.Defeats
            });

            return Ok(response);
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await utilityService.GetUser();
            var battles = await context.Battles
                .Where(u => u.AttackerId == user.Id || u.OpponentId == user.Id)
                .Include(b => b.Attacker)
                .Include(b => b.Opponent)
                .Include(b => b.Winner)
                .ToListAsync();
            var history = battles.Select(b => new BattleHistoryEntry
            {
                BattleId = b.Id,
                AttackerId = b.AttackerId,
                OpponentId = b.OpponentId,
                YouWon = b.WinnerId == user.Id,
                AttackerName = b.Attacker.Username,
                OpponentName = b.Opponent.Username,
                RoundsFight = b.RoundsFight,
                WinnerDamageSum = b.WinnerDamage,
                BattleDate = b.BattleDate
            });

            return Ok(history.OrderByDescending(b=>b.BattleDate));
        }
    }
}
