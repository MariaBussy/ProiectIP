using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseDLL
{
    public enum UserStats
    {
        Manager = 0,
        TeamLeader = 1,
        Developer = 2
    }
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public UserStats UserStat { get; set; }
        public int? TeamLeadId { get; set; }

        public List<Task> Tasks { get; set; }

    }
}
