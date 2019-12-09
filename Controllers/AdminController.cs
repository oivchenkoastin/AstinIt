using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AstinIt.Areas;

namespace AstinIt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET
        public ActionResult Index()
        {
            using (UserContext db = new UserContext())
            {
                var userId = CommonUtils.GetCurrentUserDbId();
                ViewBag.projects = db.Project.ToList();

            }


            return View();
        }

        public ActionResult InfoSection()
        {
            List<Info> Info = null;
            this.UsingDb(db => { Info = db.Info.Include(i => i.Language).ToList(); });

            return View(Info);
        }


        [HttpGet]
        public ActionResult Info(Guid id)
        {
            Info Info = null;
            this.UsingDb(db => { Info = db.Info.FirstOrDefault(c => c.Id == id); });
            return View(Info);
        }

        [HttpPost]
        public ActionResult Info(Info model)
        {
            this.UsingDb(db =>
            {
                if (db.Info.Any(i => i.Name == model.Name))
                {
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator.");

                }
                else
                {
                    db.Update(model);

                    db.SaveChanges();
                    CacheData.RefreshInfoCache();

                }
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Info model)
        {
            this.UsingDb(db =>
            {
                try
                {
                    if (db.Info.Any(i => i.Name == model.Name && i.LanguageId == model.LanguageId))
                    {
                        ModelState.AddModelError("",
                            "Localization for that string already exists");

                    }
                    else
                    {
                        db.Add(model);
                        db.SaveChanges();
                        CacheData.RefreshInfoCache();

                    }

                }
                catch (DataException)
                {
                    //Log the error (add a variable name after DataException) 
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            });
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new Info { Id = id };
            this.UsingDb(db =>
            {
                db.Info.Attach(model);
                db.Info.Remove(model);
                db.SaveChanges();
                CacheData.RefreshInfoCache();
            });

            return Redirect("/Info/InfoSection");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PageData(PageData model)
        {
            this.UsingDb(db =>
            {
                db.Update(model);
                db.SaveChanges();

                if (db.PageData.Any(i => i.Name == model.Name && i.LanguageId == model.LanguageId))
                {
                    //ModelState.AddModelError("", "Localization of this page already exists.");

                }
                else
                {
                }
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult PageData(string name)
        {
            PageData PageData = null;
            this.UsingDb(db =>
            {
                PageData = CommonUtils.GetPageData(db, name);
            });
            return View(PageData);
        }

    }
}