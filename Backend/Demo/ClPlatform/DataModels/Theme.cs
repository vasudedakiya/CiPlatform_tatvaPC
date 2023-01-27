using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Theme
    {
        public Theme()
        {
            Missions = new HashSet<Mission>();
        }

        public int Id { get; set; }
        public string ThemeName { get; set; } = null!;

        public virtual ICollection<Mission> Missions { get; set; }
    }
}
