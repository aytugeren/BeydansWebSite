using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class GalleryModel
    {
        public System.Guid Id { get; set; }
        public string PictureName { get; set; }
        public string PictureUrl { get; set; }
        public string PictureDisplayOrder { get; set; }
    }
}