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
    public class ProjectController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return ProjectSection();
        }

        public ActionResult ProjectSection()
        {
            List<Project> Project = null;
            this.UsingDb(db => { Project = db.Project.Include(p => p.ApplicationUser).ToList(); });

            return View(Project);
        }

        public ActionResult UserProjectSection()
        {
            string searchText = Request["s"];
            List<Project> Project = null;
            var userId = CommonUtils.GetCurrentUserDbId();
            ViewBag.s = string.Empty;
            this.UsingDb(db =>
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Project = db.Project.Include(p => p.ApplicationUser).Where(p => p.ApplicationUserId == userId)
                        .ToList();
                }
                else
                {
                    Project = db.Project.Include(p => p.ApplicationUser)
                        .Where(p => p.ApplicationUserId == userId
                                    && (p.Name.Contains(searchText) || p.Description.Contains(searchText)))
                        .ToList();

                    ViewBag.s = searchText;
                }
            });

            return View(Project);
        }

        [HttpGet]
        public ActionResult UserProject(Guid id)
        {
            Project Project = null;
            this.UsingDb(db =>
            {
                Project = db.Project.Include(p => p.ApplicationUser).FirstOrDefault(c => c.Id == id);
                if (Project != null)
                {
                    Project.ProjectCost =
                        db.ProjectCost.Include(p => p.Currency).Where(c => c.ProjectId == id).ToList();
                }
            });

            if (Project == null)
            {
                Redirect("/");
            }

            return View(Project);
        }

        [HttpGet]
        public ActionResult Project(Guid id)
        {
            Project Project = null;
            TempData["EnterProjectId"] = id.ToString();
            this.UsingDb(db =>
            {
                Project = db.Project.Include(p => p.ApplicationUser).FirstOrDefault(c => c.Id == id);
                if (Project != null)
                {
                    Project.ProjectCost =
                        db.ProjectCost.Include(p => p.Currency).Where(c => c.ProjectId == id).ToList();
                }
            });

            if (Project == null)
            {
                Redirect("/");
            }

            return View(Project);
        }

        [HttpPost]
        public ActionResult Project(Project model)
        {
            this.UsingDb(db =>
            {
                db.Update(model);

                db.SaveChanges();
            });

            return Project(model.Id);
        }

        [HttpPost]
        public ActionResult Create(Project model)
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
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new Project {Id = id};
            this.UsingDb(db =>
            {
                db.Project.Attach(model);
                db.Project.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/Project/ProjectSection");
        }
    }
}