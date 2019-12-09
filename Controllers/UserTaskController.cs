using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using AstinIt;
using AstinIt.Areas;

namespace AstinIt.Controllers
{
    [Authorize]
    public class UserTaskController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return UserTaskSection();
        }

        public ActionResult UserTaskSection()
        {
            List<UserTask> UserTask = null;
            this.UsingDb(db => { UserTask = db.UserTask.Include(p => p.Customer).Include(p => p.Employee).ToList(); });

            return View(UserTask);
        }

        public ActionResult UserUserTaskSection()
        {
            string searchText = Request["s"];
            List<UserTask> UserTask = null;
            var userId = CommonUtils.GetCurrentUserDbId();
            ViewBag.s = string.Empty;
            this.UsingDb(db =>
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    UserTask = db.UserTask.Include(p => p.Customer).Include(p => p.Employee)
                        .ToList();
                }
                else
                {
                    UserTask = db.UserTask.Include(p => p.Customer).Include(p => p.Employee)
                        .Where(p => (p.Name.Contains(searchText) || p.Description.Contains(searchText)))
                        .ToList();

                    ViewBag.s = searchText;
                }
            });

            return View(UserTask);
        }

        [HttpGet]
        public ActionResult UserUserTask(Guid id)
        {
            UserTask UserTask = null;
            this.UsingDb(db =>
            {
                UserTask = db.UserTask.Include(p => p.Customer).Include(p => p.Employee).FirstOrDefault(c => c.Id == id);
            });

            if (UserTask == null)
            {
                Redirect("/");
            }

            return View(UserTask);
        }

        [HttpGet]
        public ActionResult UserTask(Guid id)
        {
            UserTask UserTask = null;
            TempData["EnterUserTaskId"] = id.ToString();
            this.UsingDb(db =>
            {
                UserTask = db.UserTask.Include(p => p.Customer).Include(p => p.Employee).FirstOrDefault(c => c.Id == id);
            });

            if (UserTask == null)
            {
                Redirect("/");
            }

            return View(UserTask);
        }

        [HttpPost]
        public ActionResult UserTask(UserTask model)
        {
            this.UsingDb(db =>
            {
                db.Update(model);

                db.SaveChanges();
            });

            return UserTask(model.Id);
        }

        [HttpPost]
        public ActionResult Create(UserTask model)
        {
            this.UsingDb(db =>
            {
                try
                {
                    db.Add(model);
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    //Log the error (add a variable name after DataException) 
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            });
            return Redirect("/usertask/usertask/"+model.Id);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new UserTask {Id = id};
            this.UsingDb(db =>
            {
                db.UserTask.Attach(model);
                db.UserTask.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/UserTask/UserTaskSection");
        }
    }
}