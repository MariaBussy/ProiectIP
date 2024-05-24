using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseDLL
{
    /// <summary>
    /// Enumerație pentru starea utilizatorului.
    /// </summary>
    public enum UserStats
    {
        Manager = 0,
        TeamLeader = 1,
        Developer = 2
    }

    /// <summary>
    /// Clasă care reprezintă un utilizator în sistemul de baze de date.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public UserStats UserStat { get; set; }
        public int? TeamLeadId { get; set; }
        public string Username { get; set; } 
        public string Password { get; set; } 

        public List<Task> Tasks { get; set; }

    }
}
