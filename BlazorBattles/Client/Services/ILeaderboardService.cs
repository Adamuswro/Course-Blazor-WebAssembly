using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public interface ILeaderboardService
    {
        public List<UserStatistics> Leaderboard { get; set; }
        Task GetLeaderboard();

    }

}
