using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Mission
    {
        public Mission()
        {
            MissionImgs = new HashSet<MissionImg>();
            MissionSkills = new HashSet<MissionSkill>();
            MissionUsers = new HashSet<MissionUser>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string MissionName { get; set; } = null!;
        public string ShortDesc { get; set; } = null!;
        public string DetailDesc { get; set; } = null!;
        public int Seats { get; set; }
        public int CityId { get; set; }
        public int ThemeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationDesc { get; set; }
        public int WorkDayId { get; set; }
        public string? SponcerDetail { get; set; }
        public int IsApproved { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual Theme Theme { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual WorkDay WorkDay { get; set; } = null!;
        public virtual Story? Story { get; set; }
        public virtual ICollection<MissionImg> MissionImgs { get; set; }
        public virtual ICollection<MissionSkill> MissionSkills { get; set; }
        public virtual ICollection<MissionUser> MissionUsers { get; set; }
    }
}
