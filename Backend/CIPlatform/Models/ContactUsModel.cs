using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Models
{
    public class ContactUsModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
