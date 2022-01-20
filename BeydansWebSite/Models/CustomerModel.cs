using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class CustomerModel
    {
        public System.Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMessage { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSubscriber { get; set; }
    }
}