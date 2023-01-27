using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class MissionImg
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MissionId { get; set; }
        public string ImgUrl { get; set; } = null!;

        public virtual Mission Mission { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
