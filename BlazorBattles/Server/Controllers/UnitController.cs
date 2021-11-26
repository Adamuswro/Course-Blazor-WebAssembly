using BlazorBattles.Server.Data;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DataContext context;

        public UnitController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var units = await context.Units.ToListAsync();
            return Ok(units);
        }

        [HttpPost]
        public async Task<IActionResult> AddUnit(Unit unit)
        {
            context.Units.Add(unit);
            await context.SaveChangesAsync();

            return Ok(await context.Units.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            var dbUnit = await context.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given Id doesn't exist.");
            }

            dbUnit.Title = unit.Title;
            dbUnit.Defense = unit.Defense;
            dbUnit.Attack = unit.Attack;
            dbUnit.BananaCost = unit.BananaCost;
            await context.SaveChangesAsync();

            return Ok(dbUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var dbUnit = await context.Units.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given Id doesn't exist.");
            }

            context.Remove(dbUnit);
            await context.SaveChangesAsync();

            return Ok(await context.Units.ToListAsync());
        }
    }


}
