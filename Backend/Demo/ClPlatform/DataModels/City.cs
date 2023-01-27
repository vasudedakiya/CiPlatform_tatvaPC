using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class City
    {
        public City()
        {
            Missions = new HashSet<Mission>();
            Volunteers = new HashSet<Volunteer>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; } = null!;

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
