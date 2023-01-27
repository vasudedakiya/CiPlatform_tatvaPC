using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class WorkDay
    {
        public WorkDay()
        {
            Missions = new HashSet<Mission>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual Volunteer? Volunteer { get; set; }
        public virtual ICollection<Mission> Missions { get; set; }
    }
}
