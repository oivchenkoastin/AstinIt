namespace AstinIt
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AstinIt.UserContext";
        }

        protected override void Seed(UserContext db)
        {
            if (db.Roles.Any())
            {
                return;
            }

            var ru = db.Language.Add(new Language { Name = "Руский", Code = "ru" });
            var en = db.Language.Add(new Language { Name = "English", Code = "en" });

            string[] roles = new string[] { "Admin", "AstinItor" };
            foreach (string role in roles)
            {
                if (!db.Roles.Any(r => r.Name == role))
                {
                    db.Roles.Add(new IdentityRole(role));
                }
            }

            string adminUserID = string.Empty;
            string userID = string.Empty;
            //create user UserName:Owner Role:Admin

            var store = new UserStore<ApplicationUser>(db);
            var manager = new ApplicationUserManager(store);
            var adminUser = new ApplicationUser() { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
            manager.Create(adminUser, "admin");
            manager.AddToRole(adminUser.Id, "Admin");

            userID = adminUser.Id;


            var user = new ApplicationUser() { Email = "adminq@gmail.com", UserName = "adminq@gmail.com" };
            manager.Create(user, "admin");
            manager.AddToRole(user.Id, "AstinItor");

            adminUserID = user.Id;

            var _ukraine = db.Country.Add(new Country { Name = "Украина" });
            var _usa = db.Country.Add(new Country { Name = "Usa" });
            var _odessa = db.City.Add(new City { Name = "Одесса", Country = _ukraine });
            var _ny = db.City.Add(new City { Name = "NY", Country = _usa });


            var p1 = db.Project.Add(new Project { Name = "First project", Active = true, ApplicationUserId = userID });
            db.Project.Add(new Project { Name = "Second project", Active = true, ApplicationUserId = userID });
            db.Project.Add(new Project { Name = "Third project", Active = true, ApplicationUserId = adminUserID });

            var cur1 = db.Currency.Add(new Currency() { Name = "USD", Ratio = 1 });

            var c1 = db.ProjectCost.Add(
                new ProjectCost
                {
                    Name = "Base car cost",
                    Value = "100",
                    Description = "Base car cost from auction",
                    Currency = cur1,
                    Project = p1
                });
            var c2 = db.ProjectCost.Add(
                new ProjectCost
                {
                    Name = "Transfer",
                    Value = "1000",
                    Description = "Transfer go go",
                    Currency = cur1,
                    Project = p1
                });


            var manuSer = db.Info.Add(new Info { Name = CacheData.Info_SiteMenuServices, Value = "Services", LanguageId = en.Id });
            var manuAbo = db.Info.Add(new Info { Name = CacheData.Info_SiteMenuAbout, Value = "About", LanguageId = en.Id });
            var manuCon = db.Info.Add(new Info { Name = CacheData.Info_SiteMenuContacts, Value = "Contacts", LanguageId = en.Id });
            var manuHom = db.Info.Add(new Info { Name = CacheData.Info_SiteMenuHome, Value = "Home", LanguageId = en.Id });

            var info = db.Info.Add(new Info { Name = CacheData.Info_SiteHeaderPhone, Value = "+380 (67) 347 8888", LanguageId = en.Id });
            var info2 = db.Info.Add(new Info { Name = CacheData.Info_SiteHeaderMail, Value = "v.kudrenko@gmail.com", LanguageId = en.Id });
            var info3 = db.Info.Add(new Info { Name = CacheData.Info_SiteHeaderAddress, Value = "Odessa, UA (Genuezka st. 24D)", LanguageId = en.Id });
            var info4 = db.Info.Add(new Info { Name = CacheData.Info_SiteHeaderClocks, Value = "Mon – Sat: 9:00am–18:00pm.", LanguageId = en.Id });
            var info5 = db.Info.Add(new Info { Name = CacheData.Info_SiteFooterLeft, 
                Value = @"TopAstinIt leads the industry in wealth management. Our independent RIA and broker services have over 20 years of industry experience.", LanguageId = en.Id });



            var about = db.PageData.Add(new PageData { Name = "About", Title = "About us", Header = "About us", BodyText = "Body", LanguageId = en.Id }); //TODO replace Name hardcode
            var Contacts = db.PageData.Add(new PageData { Name = "Contacts", Title = "Contacts", Header = "Contacts", BodyText = "Body", LanguageId = en.Id }); 
            var Services = db.PageData.Add(new PageData { Name = "Services", Title = "Services", Header = "Services", BodyText = "Body", LanguageId = en.Id });

            var aboutRu = db.PageData.Add(new PageData { Name = "About", Title = "О нас", Header = "О нас", BodyText = "Body", LanguageId = ru.Id });
            var Contactsru = db.PageData.Add(new PageData { Name = "Contacts", Title = "Контакты", Header = "Контакты", BodyText = "Body", LanguageId = ru.Id });
            var Servicesru = db.PageData.Add(new PageData { Name = "Services", Title = "Сервисы", Header = "Сервисы", BodyText = "Body", LanguageId = ru.Id });

        }
    }
}
