using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace Sheep
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{v1}/{controller}/{objectId}",
               defaults: new { objectId = RouteParameter.Optional, v1 = RouteParameter.Optional }
           );
        }
    }
}
