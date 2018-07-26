using LoggingDemo.Core;
using LoggingDemo.Core.Attributes;
using System;
using System.Collections.Generic;
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
            Helpers.LogWebDiagnostic(Constants.ProductName, Constants.LayerName,
                "Just checking in....",
                new Dictionary<string, object> { { "Very", "Important" } });

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}