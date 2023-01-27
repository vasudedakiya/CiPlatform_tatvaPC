using CIPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIPlatform.Models
{
    public class AddMissionModel
    {
        public Mission? mission { get; set; }

        public GoalMission? goalMission { get; set; }

        public List<SelectListItem>? skills { get; set; }

        public List<SelectListItem>? themes { get; set; }

        public List<SelectListItem>? citys { get; set; }

        public List<SelectListItem>? countrys { get; set; }

        public List<string>? addSkill { get; set; }

        //public MissionDocument? missionDocument { get; set; }

        //public MissionMedium? missionMedia { get; set; }

        //public MissionSkill? missionSkill { get; set; }

    }
}
