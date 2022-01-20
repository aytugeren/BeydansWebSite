using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeydansWebSite.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHashKey { get; set; }
        public DateTime? LastVisitedDate { get; set; }
    }
}