using CIPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIPlatform.Models
{
    public class AddStoryModel
    {
        public List<SelectListItem> missions { get; set; }

        public Story story { get; set; }
    }
}
