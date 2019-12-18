using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Global.Setting
{
    public static class HtmlUtility
    {
          public static string IsActive(this UrlHelper html,
                                  string control,
                                  string action)
          {
            var routeAction = (string)html.RequestContext.RouteData.Values["controller"];
            var routeControl = (string)html.RequestContext.RouteData.Values["action"];

            // must match both
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
    }
}