using System.Web;
using System.Web.Mvc;

namespace Shamana.Tracing.Core.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
