using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class Story
    {
        public Story()
        {
            StoryMedia = new HashSet<StoryMedium>();
        }

        public long StoryId { get; set; }
        public long UserId { get; set; }
        public long MissionId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int ViewCount { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Mission Mission { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<StoryMedium> StoryMedia { get; set; }
    }
}
