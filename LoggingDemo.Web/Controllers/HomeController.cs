using LoggingDemo.Core.Attributes;
using System;
using System.Web.Mvc;

namespace LoggingDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            throw new Exception("Can't seem to get it right....");
            return View();
        }

        [TrackUsage(Constants.ProductName, Constants.LayerName, "View About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [TrackPerformance(Constants.ProductName, Constants.LayerName)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}