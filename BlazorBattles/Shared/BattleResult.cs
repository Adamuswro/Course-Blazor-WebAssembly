using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Shared
{
    public class BattleResult
    {
        public List<string> Log { get; set; } = new();
        public int AttackerDamageSum { get; set; }
        public int OpponentDamageSum { get; set; }
        public bool IsVisctory { get; set; }
        public int RoundsFights { get; set; }
    }
}
