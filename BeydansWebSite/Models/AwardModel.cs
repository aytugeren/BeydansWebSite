using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class AwardModel
    {
        public Guid Id { get; set; }
        public string AwardName { get; set; }
        public string AwardDescription { get; set; }
        public string AwardPicture { get; set; }
    }
}