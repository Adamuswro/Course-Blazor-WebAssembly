using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient http;

        public LeaderboardService(HttpClient http)
        {
            this.http = http;
        }
        public List<UserStatistics> Leaderboard { get; set; }

        public async Task GetLeaderboard()
        {
            Leaderboard = await http.GetFromJsonAsync<List<UserStatistics>>("api/user/leaderboard");
        }
    }
}
