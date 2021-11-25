using BlazorBattles.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public interface IUnitService
    {
        IList<Unit> Units { get; set; }
        IList<UserUnit> MyUnits { get; set; }
        void AddUnit(int UnitId);
        Task LoadUnitsAsync();
    }
}
