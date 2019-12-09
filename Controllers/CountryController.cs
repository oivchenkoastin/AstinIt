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
    public class CountryController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return CountrySection();
        }
        
        public ActionResult CountrySection()
        {
            List<Country> country = null;
            this.UsingDb(db => { country = db.Country.ToList(); });
            
            return View(country);
        }

        [HttpGet]
        public ActionResult Country(Guid id)
        {
            Country country = null;
            this.UsingDb(db => { country = db.Country.Find(id); });
            return View(country);
        }
        
        [HttpPost] 
        public ActionResult Country(Country model) 
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
        public ActionResult Create(Country model) 
        { 
            this.UsingDb(db =>
            {
                try 
                { 
                    if (ModelState.IsValid) 
                    { 
                        db.Add(model);
                        db.SaveChanges(); 
                        RedirectToAction("CountrySection"); 
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
            var model = new Country {Id = id};
            this.UsingDb(db =>
            {
                db.Country.Attach(model);
                db.Country.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/city/countrySection");
        }
        
    }
}