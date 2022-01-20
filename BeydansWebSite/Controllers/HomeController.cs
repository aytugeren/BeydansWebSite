using BeydansWebSite.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeydansWebSite.Controllers
{
    public class HomeController : Controller
    {

        BeydansDBEntities db = new BeydansDBEntities();
        public ActionResult Index()
        {
            var header = db.tblHeader.FirstOrDefault();
            var model = new HeaderModel()
            {
                Header = header.Header,
                HeaderOfTitle = header.HeaderOfTitle,
                Id = header.Id
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult HomeAbout()
        {
            var aboutDB = db.tblAbout.FirstOrDefault();
            var model = new AboutModel
            {
                AboutHeader = aboutDB.AboutHeader,
                AboutTitle = aboutDB.AboutTitle,
                AboutPicture = aboutDB.AboutPicture,
                Id = aboutDB.Id
            };

            return PartialView(model);
        }

        public PartialViewResult HomeActivities()
        {
            var awardsDB = db.tblAwards.ToList();
            var model = new List<AwardModel>();

            foreach (var item in awardsDB)
            {
                var result = new AwardModel()
                {
                    AwardDescription = item.AwardDescription,
                    AwardName = item.AwardName,
                    AwardPicture = item.AwardPicture,
                    Id = item.Id
                };

                model.Add(result);
            }

            return PartialView(model);
        }

        public PartialViewResult HomeGallery()
        {
            var list = db.tblActivities.ToList();
            var activities = new List<ActivitiesModel>();

            foreach(var item in list)
            {
                var model = new ActivitiesModel()
                {
                    ActivitiesDescription = item.ActivitiesDescription,
                    ActivitiesName = item.ActivitiesName,
                    ActivitiesPictureOrBrochure = item.ActivitiesPictureOrBrochure,
                    Id = item.Id
                };

                activities.Add(model);
            }

            return PartialView(activities);
        }

        [HttpPost]
        public ActionResult HomeContact(CustomerModel customerModel)
        {
            if (!string.IsNullOrEmpty(customerModel.CustomerEmail))
            {
                var model = new tblCustomer()
                {
                    CustomerEmail = customerModel.CustomerEmail,
                    CustomerMessage = customerModel.CustomerMessage,
                    CustomerName = customerModel.CustomerName,
                    CreatedDateTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    IsSubscriber = false
                };

                db.tblCustomer.Add(model);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("~/Shared/Error");
            }

            return RedirectToAction("Index");
        }

        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public PartialViewResult FooterAbout()
        {
            var aboutDB = db.tblAbout.FirstOrDefault();
            var model = new AboutModel
            {
                AboutHeader = aboutDB.AboutHeader,
                AboutTitle = aboutDB.AboutTitle,
                AboutPicture = aboutDB.AboutPicture,
                Id = aboutDB.Id
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult FooterSubscriber(CustomerModel customer)
        {
            var model = new tblCustomer()
            {
                CustomerEmail = customer.CustomerEmail,
                CreatedDateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                IsActive = true,
                IsDeleted = false,
                IsSubscriber = true
            };

            db.tblCustomer.Add(model);
            db.SaveChanges();

            return Json("success",JsonRequestBehavior.AllowGet);
        }
    }
}