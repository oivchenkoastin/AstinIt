using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AstinIt.Areas
{
    public static class CommonUtils
    {
        public static string GetCurrentUserName()
        {
            string username = string.Empty;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                username = System.Web.HttpContext.Current.User.Identity.Name;
            }

            return username;
        }

        public static string GetCurrentUserDbId()
        {
            return GetCurrentApplicationUser()?.Id;
        }

        private static string GetContextUserId()
        {
            var contextUserId = string.Empty;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                contextUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }

            return contextUserId;
        }
        public static PageData GetPageData(UserContext db, string name)
        {
            var PageData = db.PageData.Localized().FirstOrDefault(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
            if (PageData == null)
            {
                PageData = db.PageData.Localized("en").FirstOrDefault(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
            }

            return PageData;
        }

        public static ApplicationUser GetCurrentContextApplicationUser()
        {
            ApplicationUser currentUser;
            using (var db = new UserContext())
            {
                var store = new UserStore<ApplicationUser>(db);
                var manager = new ApplicationUserManager(store);
                var id = GetContextUserId();
                currentUser = manager.FindById(id);
            }
            return currentUser;
        }

        public static ApplicationUser GetCurrentApplicationUser()
        {
            ApplicationUser currentUser;
            using (var db = new UserContext())
            {
                var id = GetContextUserId();
                currentUser = db.Users.FirstOrDefault(x => x.Id == id);
            }
            return currentUser;
        }
    }
}