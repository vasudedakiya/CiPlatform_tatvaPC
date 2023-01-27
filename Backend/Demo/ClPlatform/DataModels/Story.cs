using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Story
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MissionId { get; set; }
        public string StoryTitle { get; set; } = null!;
        public string StoryDesc { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? VideoUrl { get; set; }
        public string? Photo { get; set; }

        public virtual Mission IdNavigation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
