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
    public class CityController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return CitySection();
        }

        public ActionResult CitySection()
        {
            List<City> City = null;
            this.UsingDb(db => { City = db.City.ToList(); });

            return View(City);
        }


        [HttpGet]
        public ActionResult City(Guid id)
        {
            City City = null;
            this.UsingDb(db => { City = db.City.Include(c => c.Country).FirstOrDefault(c => c.Id == id); });
            return View(City);
        }

        [HttpPost]
        public ActionResult City(City model)
        {
            this.UsingDb(db =>
            {
                db.Update(model);

                db.SaveChanges();
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(City model)
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
            var model = new City {Id = id};
            this.UsingDb(db =>
            {
                db.City.Attach(model);
                db.City.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/city/citySection");
        }
    }
}