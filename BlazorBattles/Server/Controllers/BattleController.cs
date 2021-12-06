using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (opponent == null || opponent.IsDeleted)
            {
                return NotFound("Opponent not avaliable!");
            }

            var result = new BattleResult();
            await Fight(attacker, opponent, result);

            return Ok(result);
        }

        private async Task Fight(User attacker, User opponent, BattleResult result)
        {
            var attackerArmy = await context.UserUnits
                .Where(u => u.UserId == attacker.Id && u.HitPoints > 0)
                .Include(u => u.Unit)
                .ToListAsync();

            var opponentArmy = await context.UserUnits
                .Where(u => u.UserId == opponent.Id && u.HitPoints > 0)
                .Include(u => u.Unit)
                .ToListAsync();

            var attackerDamageSum = 0;
            var opponentDamageSum = 0;

            int currentRound = 0;

            while (attackerArmy.Count > 0 && opponentArmy.Count > 0)
            {
                currentRound++;
                if (currentRound % 2 != 0)
                {
                    attackerDamageSum += FightRound(attacker, opponent, attackerArmy, opponentArmy, result);
                }
                else
                {
                    opponentDamageSum += FightRound(opponent, attacker, opponentArmy, attackerArmy, result);
                }
            }

            result.IsVisctory = opponentArmy.Count == 0;
            result.RoundsFights = currentRound;
            if (result.RoundsFights > 0)
                await FinishFight(attacker, opponent, result, attackerDamageSum, opponentDamageSum);
        }

        private async Task FinishFight(User attacker, User opponent, BattleResult result, int attackerDamageSum, int opponentDamageSum)
        {
            result.AttackerDamageSum = attackerDamageSum;
            result.OpponentDamageSum = opponentDamageSum;

            attacker.Battles++;
            opponent.Battles++;

            if (result.IsVisctory)
            {
                attacker.Victories++;
                opponent.Defeats++;
                attacker.Bananas += opponentDamageSum * 10;
                opponent.Bananas += attackerDamageSum ;
            }
            else
            {
                attacker.Defeats++;
                opponent.Victories++;
                attacker.Bananas += opponentDamageSum ;
                opponent.Bananas += attackerDamageSum * 10;
            }

            StoreBattleHistory(attacker, opponent, result);

            await context.SaveChangesAsync();
        }

        private void StoreBattleHistory(User attacker, User opponent, BattleResult result)
        {
            var battle = new Battle();
            battle.Attacker = attacker;
            battle.Opponent= opponent;
            battle.RoundsFight = result.RoundsFights;
            battle.WinnerDamage = result.IsVisctory ? result.AttackerDamageSum : result.OpponentDamageSum;
            battle.Winner = result.IsVisctory ? attacker : opponent;

            context.Battles.Add(battle);
        }

        private int FightRound(User opponent, User attacker, List<UserUnit> opponentArmy, List<UserUnit> attackerArmy, BattleResult result)
        {
            int randomAttackerIndex = new Random().Next(attackerArmy.Count);
            int randomOpponentIndex = new Random().Next(opponentArmy.Count);

            var randomAttacker = attackerArmy[randomAttackerIndex];
            var randomOpponent = opponentArmy[randomOpponentIndex];

            var damage = new Random().Next(Math.Max(randomAttacker.Unit.Attack - new Random().Next(randomOpponent.Unit.Defense),0));
            if (damage <= randomOpponent.HitPoints)
            {
                randomOpponent.HitPoints -= damage;
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} attacks " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title} with {damage} damage.");
            }
            else
            {
                damage = randomOpponent.HitPoints;
                randomOpponent.HitPoints = 0;
                opponentArmy.Remove(randomOpponent);
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} kills " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title}!");
            }
            return damage;
        }
    }
}
