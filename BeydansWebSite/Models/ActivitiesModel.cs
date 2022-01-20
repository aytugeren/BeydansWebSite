using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class ActivitiesModel
    {
        public System.Guid Id { get; set; }
        public string ActivitiesName { get; set; }
        public string ActivitiesDescription { get; set; }
        public string ActivitiesPictureOrBrochure { get; set; }
    }
}