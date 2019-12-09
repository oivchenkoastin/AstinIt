using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using AstinIt.Areas;
using System.Data.Entity;

namespace AstinIt.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            PageData PageData = null;
            this.UsingDb(
                db =>
                {
                    ViewBag.Vacancies = db.Vacancy.ToList();
                    ViewBag.Articles = db.Article.ToList();
                    ViewBag.UserTask = db.UserTask.ToList();
                });

            return View();
        }

        public ActionResult Panel()
        {
            using (UserContext db = new UserContext())
            {
                var userId = CommonUtils.GetCurrentUserDbId();
                ViewBag.projects = db.Project.Where(p => p.ApplicationUserId == userId).ToList();
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Language(string name)
        {
            HttpContext.Response.Cookies["Language"].Value = name;

            return Redirect("/");
        }
               

        [AllowAnonymous]
        public ActionResult Services()
        {
            PageData PageData = null;
            this.UsingDb(
                db =>
                {
                    PageData = CommonUtils.GetPageData(db, "Services");
                });

            return View("PageData", PageData);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            PageData PageData = null;
            this.UsingDb(db =>
            {
                PageData = CommonUtils.GetPageData(db, "About");
            });

            return View("PageData", PageData);
        }

        [AllowAnonymous]
        public ActionResult Contacts()
        {
            PageData PageData = null;
            this.UsingDb(db =>
            {
                PageData = CommonUtils.GetPageData(db, "Contacts");
            });

            return View("PageData", PageData);
        }

    }
}