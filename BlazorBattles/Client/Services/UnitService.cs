using BlazorBattles.Shared;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService toastService;
        private readonly HttpClient http;

        public UnitService(IToastService toastService, HttpClient http)
        {
            this.toastService = toastService;
            this.http = http;
        }

        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>
        {
            new UserUnit{HitPoints=100,UnitId=1}
        };
        //public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        public IList<Unit> Units { get; set; } = new List<Unit>();

        public void AddUnit(int UnitId)
        {
            var unit = Units.First(u => u.Id == UnitId);
            MyUnits.Add(new UserUnit { UnitId = unit.Id, HitPoints = unit.HitPoints });

            toastService.ShowSuccess($"Your {unit.Title} has been build!", "Unit build!");
        }

        public async Task LoadUnitsAsync()
        {
            if (!Units.Any())
            {
                Units = await http.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }
    }
}
