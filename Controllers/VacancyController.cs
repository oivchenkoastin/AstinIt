﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
 using AstinIt;
 using AstinIt.Areas;

 namespace AstinIt.Controllers
{
    [Authorize]
    public class VacancyController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return VacancySection();
        }
        
        public ActionResult VacancySection()
        {
            List<Vacancy> Vacancy = null;
            this.UsingDb(db => { Vacancy = db.Vacancy.Include(a => a.Language).ToList(); });

            return View(Vacancy);
        }

        [HttpGet]
        public ActionResult Vacancy(Guid id)
        {
            Vacancy Vacancy = null;
            this.UsingDb(db => { Vacancy = db.Vacancy.Include(a => a.Language).FirstOrDefault(a => a.Id == id); });
            return View(Vacancy);
        }
        
        [HttpPost] 
        public ActionResult Vacancy(Vacancy model) 
        { 
            this.UsingDb(db =>
            {
                try 
                { 
                    if (ModelState.IsValid) 
                    {
                        db.Update(model);
                        db.SaveChanges(); 
                    } 
                } 
                catch (DataException) 
                { 
                    //Log the error (add a variable name after DataException) 
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator."); 
                }  
                
            });
            return View(model); 
        }
        
        [HttpPost] 
        public ActionResult Create(Vacancy model) 
        { 
            this.UsingDb(db =>
            {
                try 
                { 
                    if (ModelState.IsValid) 
                    { 
                        db.Add(model);
                        db.SaveChanges(); 
                        RedirectToAction("VacancySection"); 
                    } 
                } 
                catch (DataException) 
                { 
                    //Log the error (add a variable name after DataException) 
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator."); 
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
            var model = new Vacancy {Id = id};
            this.UsingDb(db =>
            {
                db.Vacancy.Attach(model);
                db.Vacancy.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/city/VacancySection");
        }
        
    }
}