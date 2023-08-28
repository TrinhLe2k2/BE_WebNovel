using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BE_WebNovel.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        private WebNovelEntities db = new WebNovelEntities();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // 1. Check session:
            var CurrentUser = (User)HttpContext.Current.Session["user"];
            if(CurrentUser != null)
            {
                return;
            }
            else
            {
                if (filterContext.HttpContext.Request.Cookies["LoginCookie"] != null)
                {
                    string email = filterContext.HttpContext.Request.Cookies["LoginCookie"]["Email"];
                    string password = filterContext.HttpContext.Request.Cookies["LoginCookie"]["Password"];

                    var user = db.Users.SingleOrDefault(m => m.email == email && m.password == password);

                    if (user != null) 
                    {
                        filterContext.HttpContext.Session["user"] = user;
                        return;
                    }
                }

                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                var newUrl = "~/Home/Index";
                filterContext.Result = new RedirectResult(newUrl);
            }
        }
    }
}