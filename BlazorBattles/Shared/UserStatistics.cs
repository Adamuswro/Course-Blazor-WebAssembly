using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Shared
{
    public class UserStatistics
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
