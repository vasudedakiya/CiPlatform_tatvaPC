using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string? Message { get; set; }
        public string Img { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
