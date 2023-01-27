using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Volunteer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Company { get; set; }
        public string? Department { get; set; }
        public string? AboutVolunteer { get; set; }
        public string? WhyVolunteer { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int WorkDayId { get; set; }
        public string? LinkedIn { get; set; }
        public string? Photo { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual Country Country { get; set; } = null!;
        public virtual WorkDay IdNavigation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
