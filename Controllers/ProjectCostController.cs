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
    public class ProjectCostController : Controller
    {
        public ActionResult Index()
        {
            return ProjectCostSection();
        }

        public ActionResult ProjectCostSection()
        {
            List<ProjectCost> ProjectCost = null;
            this.UsingDb(db => { ProjectCost = db.ProjectCost.Include(p => p.Currency).ToList(); });

            return View(ProjectCost);
        }

        [HttpGet]
        public ActionResult UserProjectCost(Guid id)
        {
            ProjectCost ProjectCost = null;
            this.UsingDb(db =>
            {
                ProjectCost = db.ProjectCost.Include(p => p.Currency).FirstOrDefault(c => c.Id == id);
            });

            if (ProjectCost == null)
            {
                Redirect("/");
            }
            
            return View(ProjectCost);
        }

        [HttpGet]
        public ActionResult ProjectCost(Guid id)
        {
            ProjectCost ProjectCost = null;
            this.UsingDb(db =>
            {
                ProjectCost = db.ProjectCost.Include(p => p.Currency).FirstOrDefault(c => c.Id == id);
            });

            if (ProjectCost == null)
            {
                Redirect("/");
            }
            
            return View(ProjectCost);
        }

        [HttpPost]
        public ActionResult ProjectCost(ProjectCost model)
        {
            this.UsingDb(db =>
            {
                db.Update(model);

                db.SaveChanges();
            });

            return ProjectCost(model.Id);
        }

        [HttpPost]
        public ActionResult Create(ProjectCost model)
        {
            this.UsingDb(db =>
            {
                try
                {
                    model.ProjectId = new Guid((string)TempData["EnterProjectId"]);
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
            var model = new ProjectCost {Id = id};
            this.UsingDb(db =>
            {
                db.ProjectCost.Attach(model);
                db.ProjectCost.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/ProjectCost/ProjectCostSection");
        }
    }
}