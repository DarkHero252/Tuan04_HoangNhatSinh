using System.Web;
using System.Web.Mvc;

namespace Tuan04_HoangNhatSinh
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
