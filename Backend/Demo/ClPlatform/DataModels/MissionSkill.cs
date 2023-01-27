using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class MissionSkill
    {
        public int Id { get; set; }
        public int MissionId { get; set; }
        public int SkillId { get; set; }

        public virtual Mission Mission { get; set; } = null!;
        public virtual Skill Skill { get; set; } = null!;
    }
}
