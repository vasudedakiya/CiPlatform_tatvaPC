using System;
using System.Collections.Generic;

namespace CIPlatform.DataModels
{
    public partial class PasswordReset
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
