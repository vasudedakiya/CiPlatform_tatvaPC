using CIPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIPlatform.Models
{
    public class UserProfileModel
    {

        public User user { get; set; }

        public List<SelectListItem>? Countrys { get; set; }

        public List<SelectListItem>? Citys { get; set; }

        public List<string>? UserSkills { get; set; }

        
    }
}
