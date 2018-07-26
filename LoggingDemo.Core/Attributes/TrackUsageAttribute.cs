using System.Web.Mvc;

namespace LoggingDemo.Core.Attributes
{
    //requires NuGet package Microsoft.AspNet.Mvc
    public class TrackUsageAttribute : ActionFilterAttribute
    {
        private string _productName;
        private string _layerName;
        private string _name;

        public TrackUsageAttribute(string product, string layer, string name)
        {
            _productName = product;
            _layerName = layer;
            _name = name;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Helpers.LogWebUsage(_productName, _layerName, _name);
        }
    }
}
