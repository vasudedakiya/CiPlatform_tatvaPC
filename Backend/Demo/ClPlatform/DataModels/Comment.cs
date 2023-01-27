using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MissionId { get; set; }
        public string Comment1 { get; set; } = null!;
        public DateTime DateTime { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
