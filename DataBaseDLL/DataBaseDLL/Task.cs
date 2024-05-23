using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseDLL
{
    public class Task
    {
        public int TaskId { get; set; }
        public String NumeTask { get; set; }
        public DateTime DataAsignarii { get; set; }
        public double OreLogate { get; set; }
        public String DescriereTask { get; set; }
        public String NumeAssigner { get; set; }
    }

}
