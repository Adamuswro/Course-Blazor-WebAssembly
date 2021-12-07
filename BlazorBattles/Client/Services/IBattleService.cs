using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public interface IBattleService
    {
        public BattleResult LastBattle { get; set; }
        public Task<BattleResult> StartBattle(int opponentId);
    }
}
