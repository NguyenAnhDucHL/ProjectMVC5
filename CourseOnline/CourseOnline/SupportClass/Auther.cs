using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPWy.SupportClass
{
    public class Auther : ActionFilterAttribute, IAuthorizationFilter
    {
        private string Permission { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["Permission"])))
            //{
            //    var Url = new UrlHelper(filterContext.RequestContext);
            //    var url = Url.Action("ExternalLoginCallback", "Account");
            //    filterContext.Result = new RedirectResult(url);
            //}
            //else
            //{
            //    List<string> Permissions = (List<string>)filterContext.HttpContext.Session["Permission"];
            //    bool Check = false;
            //    foreach (var p in Permission.Split(','))
            //    {
            //        if (Permissions.Contains(p))
            //        {
            //            Check = true;
            //            break;
            //        }
            //    }
            //    if (!Check)
            //    {
            //        string url = (string)filterContext.HttpContext.Session["url"];
            //        filterContext.Result = new RedirectResult("/" + url);
            //        //filterContext.Result = new ViewResult() { ViewName = "Permisssion" };
            //    }
            //}

        }
    }
}