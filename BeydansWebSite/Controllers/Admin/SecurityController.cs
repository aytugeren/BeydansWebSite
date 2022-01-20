using BeydansWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BeydansWebSite.Controllers.Admin
{
    public class SecurityController : Controller
    {
        private readonly string saltPassword = "encryptKey";
        BeydansDBEntities db = new BeydansDBEntities();

        // GET: Security
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUser(UserModel userModel)
        {
            if (userModel != null)
            {
                var hashedPassword = this.HashPassword(userModel.Password);
                var user = db.tblUser.Where(x => x.Username == userModel.Username && x.Password == hashedPassword).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            SHA1 encrypt = new SHA1CryptoServiceProvider();
            var encrypted = encrypt.ComputeHash(Encoding.UTF8.GetBytes(password + saltPassword));
            var sb = new StringBuilder(encrypted.Length * 2);
            foreach (byte b in encrypted)
            {
                sb.Append(b.ToString("x2"));
            }
            string encryptedPassword = sb.ToString();
            return encryptedPassword;
        }
    }
}