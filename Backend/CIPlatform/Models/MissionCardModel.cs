using CIPlatform.DataModels;

namespace CIPlatform.Models
{
    public class MissionCardModel
    {
        public Mission mission { get; set; }

        public string? CardImg { get; set; }

        public int? missionRating { get; set; }

        public int? seatsLeft { get; set; } 

        public int? favMission { get; set; }

        public GoalMission? goalMission { get; set; }

        public float? progressBar { get; set; }

        public string theme { get; set; }

        public string country { get; set; }

        public float? avgRateing { get; set; }


    }
}
