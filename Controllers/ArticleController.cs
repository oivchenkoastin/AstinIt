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
    public class ArticleController : Controller, IUseDb
    {
        public ActionResult Index()
        {
            return ArticleSection();
        }
        
        public ActionResult ArticleSection()
        {
            List<Article> Article = null;
            this.UsingDb(db => { Article = db.Article.Include(a => a.Language).ToList(); });

            return View(Article);
        }

        [HttpGet]
        public ActionResult Article(Guid id)
        {
            Article Article = null;
            this.UsingDb(db => { Article = db.Article.Include(a => a.Language).FirstOrDefault(a => a.Id == id); });
            return View(Article);
        }
        
        [HttpPost] 
        public ActionResult Article(Article model) 
        { 
            this.UsingDb(db =>
            {
                try 
                { 
                    if (ModelState.IsValid)
                    {
                        model.DateModified = DateTime.Now;
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
        public ActionResult Create(Article model) 
        { 
            this.UsingDb(db =>
            {
                try 
                {
                    model.DateCreated = DateTime.Now;
                    model.DateModified = DateTime.Now;
                    if (ModelState.IsValid) 
                    { 
                        db.Add(model);
                        db.SaveChanges(); 
                    } 
                } 
                catch (DataException) 
                { 
                    //Log the error (add a variable name after DataException) 
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator."); 
                }  
                
            });
            //return RedirectToAction("ArticleSection");
            return View(model); 
        }
        
        
        public ActionResult Create()
        {
            return View();
        }
        
        
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new Article {Id = id};
            this.UsingDb(db =>
            {
                db.Article.Attach(model);
                db.Article.Remove(model);
                db.SaveChanges();
            });

            return Redirect("/city/ArticleSection");
        }
        
    }
}