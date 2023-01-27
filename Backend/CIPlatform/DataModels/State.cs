using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
        }

        public long StateId { get; set; }
        public long CountryId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<City> Cities { get; set; }
    }
}
