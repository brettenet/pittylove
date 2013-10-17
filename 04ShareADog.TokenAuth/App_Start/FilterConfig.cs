using System.Web;
using System.Web.Mvc;

namespace _04ShareADog.TokenAuth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}