using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class PropertyModel
    {
        public Guid Id { get; set; }
        public string PropertyName { get; set; }
        public string PropertyDescription { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}