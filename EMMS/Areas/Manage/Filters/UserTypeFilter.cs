using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMMS.Areas.Manage.Filters
{
    public class UserTypeFilter:FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserType"].ToString() != "0")
            {
                filterContext.Result = new RedirectResult("/Manage/Admin/UserTypeNotSupport");
            }
        }
    }
}