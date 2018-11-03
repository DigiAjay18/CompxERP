using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;

namespace CompxERP.Filters
{
    public class UserSessionfilter : ActionFilterAttribute
    {
        //
        // GET: /UserSessionfilter/

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            if (SessionMaster.UserID <= 0 || SessionMaster.CompCode <= 0)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult { Content = "SessionExpired" };
                    filterContext.Result = new RedirectResult("/Home/partnerlogin");
                }
                else
                {
                    // filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["SSFSecurity"]);
                    filterContext.Result = new RedirectResult("/Home/partnerlogin");
                }
                //controller.HttpContext.Response.Redirect("/User/UserLogin");
            }

        }

      

    }
}
