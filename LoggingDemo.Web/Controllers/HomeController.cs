using LoggingDemo.Core;
using LoggingDemo.Core.Attributes;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace LoggingDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        [TrackUsage(Constants.ProductName, Constants.LayerName, "View Home")]
        public ActionResult Index()
        {
            var db = new SqlCommand("select * from dbo.logs", new SqlConnection("connection"));
            db.ExecuteNonQuery();

            return View();
        }

        [TrackUsage(Constants.ProductName, Constants.LayerName, "View About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [TrackUsage(Constants.ProductName, Constants.LayerName, "View Contact")]
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