using System;
using System.Web;

namespace LoggingDemo.Core
{
    public static class Helpers
    {

        public static void GetHttpStatus(Exception ex, out int httpStatus)
        {
            httpStatus = 500;  // default is server error
            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                httpStatus = httpEx.GetHttpCode();
            }
        }

    }
}
