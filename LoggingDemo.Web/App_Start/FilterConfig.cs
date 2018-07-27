using LoggingDemo.Core.Attributes;
using System.Web.Mvc;

namespace LoggingDemo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TrackPerformanceAttribute(Constants.ProductName,
                Constants.LayerName));
        }
    }
}
