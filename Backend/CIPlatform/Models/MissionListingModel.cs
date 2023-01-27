using CIPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIPlatform.Models
{
    public class MissionListingModel
    {
        public List<MissionCardModel> missionsCard { get; set; }

        public List<SelectListItem> themes { get; set; }

        public List<SelectListItem> citys { get; set; }

        public List<SelectListItem> countrys { get; set; }

        public List<SelectListItem> skills { get; set; }

        public int? TotalMission { get; set; }

    }
}
