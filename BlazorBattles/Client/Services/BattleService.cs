using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient https;

        public BattleService(HttpClient https)
        {
            this.https = https;
        }

        public BattleResult LastBattle { get; set; } = new();
        public IList<BattleHistoryEntry> History { get ; set; } = new List<BattleHistoryEntry>();

        public async Task GetHistory()
        {
            History = await https.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
        }

        public async Task<BattleResult> StartBattle(int opponentId)
        {
            var result = await https.PostAsJsonAsync("api/battle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();
            return LastBattle;
        }
    }
}
