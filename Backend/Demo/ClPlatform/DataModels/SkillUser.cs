using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class SkillUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public virtual Skill Skill { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
