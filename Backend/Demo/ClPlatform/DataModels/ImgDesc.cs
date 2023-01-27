using System;
using System.Collections.Generic;

namespace ClPlatform.DataModels
{
    public partial class ImgDesc
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
