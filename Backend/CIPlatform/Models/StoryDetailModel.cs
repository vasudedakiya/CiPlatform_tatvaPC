using CIPlatform.DataModels;

namespace CIPlatform.Models
{
    public class StoryDetailModel
    {

        public string? userName { get; set; }

        public int storyId { get; set; }

        public string? userImg { get; set; }
        
        public string? whyVolenteer { get; set; }

        public int missionId { get; set; }

        public List<string> storyImg { get; set; }

        public string storyTitle { get; set; }

        public string storyDetail { get; set; }

        public int viewCount { get; set; }



    }
}
