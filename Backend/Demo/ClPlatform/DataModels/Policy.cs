using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class Policy
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string PolicyDesc { get; set; } = null!;
    }
}
