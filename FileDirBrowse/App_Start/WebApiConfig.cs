using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace FileDirBrowse
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "Get",
                "Api/{controller}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
                "Post",
                "Api/{controller}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            config.Routes.MapHttpRoute(
                "UpStairs",
                "Api/{controller}/{action}",
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
                
           config.Routes.MapHttpRoute(
                "GetRoot",
                "Api/{controller}",
                new { action = "GetRoot" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });

        }
    }
}
