using BeydansWebSite.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BeydansWebSite.Controllers.Admin
{
    public class AdminController : Controller
    {
        BeydansDBEntities db = new BeydansDBEntities();
        
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public PartialViewResult IndexSummaryCount()
        {
            var subcriberCount = db.tblCustomer.Where(x => x.IsSubscriber).Count();
            var customerCount = db.tblCustomer.Where(x => !x.IsSubscriber).Count();
            var activitiesCount = db.tblActivities.Count();
            var awardsCount = db.tblAwards.Count();

            var model = new IndexItemsCount()
            {
                ActivitiesCount = activitiesCount,
                AwardsCount = awardsCount,
                CustomerCount = customerCount,
                SubscriberCount = subcriberCount
            };

            return PartialView(model);
        }

        [Authorize]
        public ActionResult HeaderEdit()
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

        [Authorize]
        [HttpPost]
        public ActionResult EditHeaderModal(string header, string headerOfTitle, Guid id)
        {
            var headerDb = db.tblHeader.Find(id);
            headerDb.Header = header;
            headerDb.HeaderOfTitle = headerOfTitle;

            db.Entry(headerDb).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AboutPage");
        }

        [Authorize]
        public ActionResult AdminAbout()
        {
            var about = db.tblAbout.FirstOrDefault();

            var aboutModel = new AboutModel()
            {
                AboutHeader = about.AboutHeader,
                AboutPicture = about.AboutPicture,
                AboutTitle = about.AboutTitle,
                Id = about.Id
            };

            return View(aboutModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAboutPage(string aboutHeader, string aboutTitle, Guid id)
        {
            var about = db.tblAbout.Find(id);
            string yol = string.Empty;
            if (Request.Files.Count > 0 && Request.Files[0].FileName != about.AboutPicture && !string.IsNullOrEmpty(Request.Files[0].FileName))
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                if (dosyaAdi.Contains("jpg") || dosyaAdi.Contains("png"))
                {
                    yol = "/Images/" + dosyaAdi;
                }
                else
                {
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    yol = "/Images/" + dosyaAdi + uzanti;
                }

                Request.Files[0].SaveAs(Server.MapPath(yol));
                about.AboutPicture = yol;
            }

            about.AboutHeader = aboutHeader;
            about.AboutTitle = aboutTitle;
            db.Entry(about).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AdminAbout");
        }

        [Authorize]
        public ActionResult AdminEditAwards()
        {
            var awards = db.tblAwards.ToList();
            var awardsModel = new List<AwardModel>();

            foreach (var item in awards)
            {
                var model = new AwardModel
                {
                    AwardDescription = item.AwardDescription,
                    AwardName = item.AwardName,
                    AwardPicture = item.AwardPicture,
                    Id = item.Id
                };

                awardsModel.Add(model);
            }

            return View(awardsModel);
        }

        [Authorize]
        [HttpGet]
        [Route("EditAwardModal/{id}")]
        public JsonResult EditAwardModal(Guid id)
        {
            var award = db.tblAwards.Find(id);

            var model = new AwardModel
            {
                AwardDescription = award.AwardDescription,
                AwardName = award.AwardName,
                AwardPicture = award.AwardPicture,
                Id = award.Id
            };

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAwards(AwardModel award)
        {
            var awardDb = db.tblAwards.Find(award.Id);
            string yol = string.Empty;

            if (Request.Files.Count > 0 && Request.Files[0].FileName != awardDb.AwardPicture && !string.IsNullOrEmpty(Request.Files[0].FileName))
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                if (dosyaAdi.Contains("jpg") || dosyaAdi.Contains("png"))
                {
                    yol = "/Images/" + dosyaAdi;
                }
                else
                {
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    yol = "/Images/" + dosyaAdi + uzanti;
                }

                Request.Files[0].SaveAs(Server.MapPath(yol));
                awardDb.AwardPicture = yol;
            }

            awardDb.AwardName = award.AwardName;
            awardDb.AwardDescription = award.AwardDescription;
            db.Entry(awardDb).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AdminEditAwards");
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveAwardModal/{id}")]
        public JsonResult RemoveAward(Guid id)
        {
            var award = db.tblAwards.Find(id);

            if (award != null)
            {
                db.tblAwards.Remove(award);
                db.SaveChanges();
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAward(AwardModel award)
        {
            var awardDB = new tblAwards()
            {
                AwardDescription = award.AwardDescription,
                AwardName = award.AwardName,
                Id = Guid.NewGuid()
            };

            string yol = string.Empty;

            if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[0].FileName))
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                if (dosyaAdi.Contains("jpg") || dosyaAdi.Contains("png"))
                {
                    yol = "/Images/" + dosyaAdi;
                }
                else
                {
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    yol = "/Images/" + dosyaAdi + uzanti;
                }

                Request.Files[0].SaveAs(Server.MapPath(yol));
                awardDB.AwardPicture = yol;
            }

            db.tblAwards.Add(awardDB);
            db.SaveChanges();

            return RedirectToAction("AdminEditAwards");
        }

        [Authorize]
        public ActionResult EditActivities()
        {
            var list = db.tblActivities.ToList();
            var activities = new List<ActivitiesModel>();

            foreach (var item in list)
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

            return View(activities);
        }

        [Authorize]
        [HttpGet]
        [Route("EditActivity/{id}")]
        public JsonResult EditActivity(Guid id)
        {
            var activityDb = db.tblActivities.Find(id);

            var model = new ActivitiesModel()
            {
                ActivitiesDescription = activityDb.ActivitiesDescription,
                ActivitiesName = activityDb.ActivitiesName,
                ActivitiesPictureOrBrochure = activityDb.ActivitiesPictureOrBrochure,
                Id = activityDb.Id
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditActivity(ActivitiesModel activity)
        {
            if (activity != null)
            {
                var act = db.tblActivities.Find(activity.Id);
                string yol = string.Empty;

                act.ActivitiesDescription = activity.ActivitiesDescription;
                act.ActivitiesName = activity.ActivitiesName;

                if (Request.Files.Count > 0 && Request.Files[0].FileName != act.ActivitiesPictureOrBrochure && !string.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                    if (dosyaAdi.Contains("jpg") || dosyaAdi.Contains("png"))
                    {
                        yol = "/Images/" + dosyaAdi;
                    }
                    else
                    {
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        yol = "/Images/" + dosyaAdi + uzanti;
                    }

                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    act.ActivitiesPictureOrBrochure = yol;
                }

                db.Entry(act).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("EditActivities");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddActivity(ActivitiesModel activities)
        {
            if (activities != null)
            {
                var modelDb = new tblActivities()
                {
                    ActivitiesDescription = activities.ActivitiesDescription,
                    ActivitiesName = activities.ActivitiesName,
                    Id = Guid.NewGuid()
                };


                string yol = string.Empty;

                if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                    if (dosyaAdi.Contains("jpg") || dosyaAdi.Contains("png"))
                    {
                        yol = "/Images/" + dosyaAdi;
                    }
                    else
                    {
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        yol = "/Images/" + dosyaAdi + uzanti;
                    }

                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    modelDb.ActivitiesPictureOrBrochure = yol;
                }

                db.tblActivities.Add(modelDb);
                db.SaveChanges();
            }

            return RedirectToAction("EditActivities");
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveActivity/{id}")]
        public JsonResult RemoveActivity(Guid id)
        {
            var activity = db.tblActivities.Find(id);

            if (activity != null)
            {
                db.tblActivities.Remove(activity);
                db.SaveChanges();
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Customers()
        {
            var customer = db.tblCustomer.Where(x => x.IsSubscriber != true).ToList();

            var model = new List<CustomerModel>();

            foreach (var item in customer)
            {
                var c = new CustomerModel()
                {
                    CustomerEmail = item.CustomerEmail,
                    CustomerMessage = item.CustomerMessage,
                    CustomerName = item.CustomerName,
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    IsActive = item.IsActive,
                    CreatedDateTime = item.CreatedDateTime
                };

                model.Add(c);
            }


            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("GetPassiveCustomer/{id}")]
        public JsonResult GetPassiveCustomer(Guid id)
        {
            var customer = db.tblCustomer.Find(id);
            if (customer != null)
            {
                customer.IsActive = false;
                customer.IsDeleted = true;

                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json("1", JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult Subscribers()
        {
            var customerDb = db.tblCustomer.Where(x => x.IsSubscriber).ToList();

            var list = new List<CustomerModel>();

            foreach (var item in customerDb)
            {
                var model = new CustomerModel()
                {
                    CustomerEmail = item.CustomerEmail,
                    CustomerMessage = item.CustomerMessage,
                    CustomerName = item.CustomerName,
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    IsActive = item.IsActive,
                    CreatedDateTime = item.CreatedDateTime
                };

                list.Add(model);
            }

            return View(list);
        }

        [Authorize]
        public ActionResult AddPropertyForWebsite()
        {

            var listModel = new List<PropertyModel>();

            db.tblProperties.ToList().ForEach(x => listModel.Add(new PropertyModel() {
                CreatedDateTime = x.CreatedDateTime,
                EndDate = x.EndDate,
                Id = x.Id,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                PropertyDescription = x.PropertyDescription,
                PropertyName = x.PropertyName
            }));

            return View(listModel);
        }

        [Authorize]
        [HttpPost]
        [Route("SendMail/{id}")]
        public JsonResult SendMail(Guid id)
        {
            var emailContent = db.tblProperties.Find(id);
            var customer = db.tblUser.FirstOrDefault();

            var smtpClient = new SmtpClient("smtp-relay.sendinblue.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("aytgeren@gmail.com", "YJ1sw53bRXBI6d9h"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("aytgeren@gmail.com"),
                Subject = "Özellik İsteğinde Bulunuldu!",
                Body = "<p>Merhaba Admin,</p> <br /> <p>" + customer.Username + " adlı kullanıcı sizden " + emailContent.PropertyName + " - "+
                emailContent.PropertyDescription+ " adlı özellik hakkında destek istiyor.</p>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("aytgeren@gmail.com");

            smtpClient.Send(mailMessage);

            return Json("1", JsonRequestBehavior.AllowGet);
        }
    }
}