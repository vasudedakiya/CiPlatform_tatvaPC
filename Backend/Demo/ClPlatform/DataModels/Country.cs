using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
            Volunteers = new HashSet<Volunteer>();
        }

        public int Id { get; set; }
        public byte[] CountryName { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
