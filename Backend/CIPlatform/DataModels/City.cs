using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }

        public long CityId { get; set; }
        public long CountryId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
