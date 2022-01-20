using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeydansWebSite.Models
{
    public class AboutModel
    {
        public System.Guid Id { get; set; }
        public string AboutHeader { get; set; }
        [AllowHtml]
        public string AboutTitle { get; set; }
        public string AboutPicture { get; set; }
    }
}