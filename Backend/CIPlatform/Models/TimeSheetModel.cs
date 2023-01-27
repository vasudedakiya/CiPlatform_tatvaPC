using CIPlatform.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Models
{
    public class TimeSheetModel : ValidationAttribute
    {

        public List<SelectListItem>? missions { get; set; }

        public Timesheet timeSheet { get; set; }

    }
}
