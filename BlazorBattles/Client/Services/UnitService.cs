using BlazorBattles.Shared;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService toastService;
        private readonly HttpClient http;
        private readonly IBananaService bananaService;

        public UnitService(IToastService toastService, HttpClient http, IBananaService bananaService)
        {
            this.toastService = toastService;
            this.http = http;
            this.bananaService = bananaService;
        }

        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>
        {
            new UserUnit{HitPoints=100,UnitId=1}
        };
        //public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        public IList<Unit> Units { get; set; } = new List<Unit>();

        public async Task AddUnit(int UnitId)
        {
            var unit = Units.First(u => u.Id == UnitId);
            var result = await http.PostAsJsonAsync<int>("api/userunit", UnitId);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                toastService.ShowError(await result.Content.ReadAsStringAsync());
                return;
            }

            await bananaService.GetBananas();
            toastService.ShowSuccess($"Your {unit.Title} has been build!", "Unit build!");
        }

        public async Task LoadUnitsAsync()
        {
            if (!Units.Any())
            {
                Units = await http.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }

        public async Task LoadUserUnitsAsync()
        {
            MyUnits = await http.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
        }

        public async Task ReviveArmy()
        {
            var result = await http.PostAsJsonAsync<string>("api/userunit/revive", null);
            if (result.StatusCode == HttpStatusCode.OK)
                toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
            else
                toastService.ShowError(await result.Content.ReadAsStringAsync());

            await LoadUserUnitsAsync();
            await bananaService.GetBananas();
        }
    }
}
