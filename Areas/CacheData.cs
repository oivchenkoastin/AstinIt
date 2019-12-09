using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AstinIt
{
    public class CacheData
    {
        public static string Info_SiteHeaderPhone = "Info_SiteHeaderPhone";
        public static string Info_SiteHeaderAddress = "Info_SiteHeaderAddress";
        public static string Info_SiteHeaderMail = "Info_SiteHeaderMail";
        public static string Info_SiteHeaderClocks = "Info_SiteHeaderClocks";
        public static string Info_SiteFooterLeft = "Info_SiteFooterLeft";


        public static string Info_SiteMenuAbout = "Info_SiteMenuAbout";
        public static string Info_SiteMenuContacts = "Info_SiteMenuContacts";
        public static string Info_SiteMenuServices = "Info_SiteMenuServices";
        public static string Info_SiteMenuHome = "Info_SiteMenuHome";


        public static Dictionary<string, Info> Info;
        public static Dictionary<string, Language> Language;

        static CacheData()
        {
            RefreshInfoCache();
        }

        public static void RefreshInfoCache()
        {

            new object().UsingDb(db =>
            {
                Info = db.Info.Localized().ToDictionary(k => k.Name, k => k);
                Language = db.Language.ToDictionary(k => k.Name, k => k);

                
            });
        }

        public static string GetInfoFromCache(string name)
        {
            if (Info.ContainsKey(name) && !string.IsNullOrEmpty(Info[name].Value))
            {
                return Info[name].Value;
            }
            else
            {
                return name;
            }
        }
    }
}