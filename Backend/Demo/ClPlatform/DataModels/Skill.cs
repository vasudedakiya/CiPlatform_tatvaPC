using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Skill
    {
        public Skill()
        {
            MissionSkills = new HashSet<MissionSkill>();
            SkillUsers = new HashSet<SkillUser>();
        }

        public int Id { get; set; }
        public string SkillName { get; set; } = null!;

        public virtual ICollection<MissionSkill> MissionSkills { get; set; }
        public virtual ICollection<SkillUser> SkillUsers { get; set; }
    }
}
