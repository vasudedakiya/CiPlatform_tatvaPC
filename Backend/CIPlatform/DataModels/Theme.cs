using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class Theme
    {
        public Theme()
        {
            Missions = new HashSet<Mission>();
        }

        public long ThemeId { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }
    }
}
