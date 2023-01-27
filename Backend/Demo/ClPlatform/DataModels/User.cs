using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            MissionImgs = new HashSet<MissionImg>();
            MissionUsers = new HashSet<MissionUser>();
            Missions = new HashSet<Mission>();
            SkillUsers = new HashSet<SkillUser>();
            Stories = new HashSet<Story>();
            Volunteers = new HashSet<Volunteer>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNo { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<MissionImg> MissionImgs { get; set; }
        public virtual ICollection<MissionUser> MissionUsers { get; set; }
        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<SkillUser> SkillUsers { get; set; }
        public virtual ICollection<Story> Stories { get; set; }
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
