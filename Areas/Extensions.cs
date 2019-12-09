using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Data;

namespace AstinIt
{
    public static class Extensions
    {
        public static void UsingDb(this object obj, Action<UserContext> action)
        {
            using (UserContext db = new UserContext())
            {
                action(db);
            }
        }
        
        
        public static IEnumerable<BaseDbObject> Active<T>(this IEnumerable<T> obj) where T : BaseDbObject
        {
            return obj.Where(o => o.Active);
        }
        
        public static IEnumerable<T> Localized<T>(this IQueryable<T> obj, string langDefault = null) where T : LocalizableDbObject
        {
            var lang = langDefault == null ? HttpContext.Current.Request.Cookies["Language"]?.Value : langDefault;
            if (string.IsNullOrEmpty(lang))
            {
                lang = "en";
            }
            return obj.Include(i => i.Language).Where(i => i.Language.Code == lang);
        }



        public static IEnumerable<SelectListItem> ConvertToListItems(this IEnumerable<IDBObject> objects, Guid? id)
        {
            return objects.Select(obj => new SelectListItem {Text = obj.Name, Value = obj.Id.ToString(), Selected = id != null && obj.Id.Equals(id)});
        }
        
        public static IEnumerable<UserDto> ToUserDtoList(this IEnumerable<ApplicationUser> users)
        {
            return users.Select(u => u.ToUserDto());
        } 
        
        private static UserDto ToUserDto(this ApplicationUser user)
        {
            return new UserDto
            {
                Name = user.UserName,
                Id = Guid.Parse(user.Id),
                Active = user.Active
            };
        }

    }
}