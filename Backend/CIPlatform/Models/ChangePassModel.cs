using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Models
{
    public class ChangePassModel
    {
        public int UserId { get; set; }

        public string Password { get; set; }

        public string newPassword { get; set; }
    }
}
