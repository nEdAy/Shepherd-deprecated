using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Timers;
using Sheep.Models.ItemCache;

namespace Sheep
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //移除X-AspNetMvc-Version
            MvcHandler.DisableMvcResponseHeader = true;

            //设置定时器实现自动刷新
            //Timer timer = new Timer(1000 * 60 * 5);
            //timer.Elapsed += new ElapsedEventHandler(delegate { new CacheManager().RefreshCache(); });
            //timer.Enabled = true;
            //timer.AutoReset = true;

        }
    }
}
