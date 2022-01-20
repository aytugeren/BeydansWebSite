using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeydansWebSite.Controllers.Admin
{
    public class ErrorController : Controller
    {
        public ActionResult PageError()
        {
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult PageErrorHome()
        {
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}