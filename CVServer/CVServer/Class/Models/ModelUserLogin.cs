using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesta.CvServer
{
    public class ModelUserLogin
    {
        public string UserID { get; set; }
        public string TeamName { get; set; }
        public DateTime StratTime { get; set; }
        public Int16 CounterNumber { get; set; }
        public int? MuniteUses { get; set; }
    }
}
