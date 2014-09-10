using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMMS.Areas.Manage.Filters
{
    public class AuthorizeFilter:FilterAttribute,IAuthorizationFilter

    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserID"] == null)
            {
                string returnUrl ="manage" +"/"+ filterContext.RouteData.GetRequiredString("controller") + "/" + filterContext.RouteData.GetRequiredString("action");
                filterContext.Result = new RedirectResult("/Manage/Admin/SignIn?returnUrl="+returnUrl);
            }
        }
    }
}