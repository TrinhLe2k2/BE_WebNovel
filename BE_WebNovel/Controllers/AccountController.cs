using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Controllers
{
    public class AccountController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();

        // GET: Account
        public ActionResult Login(string InputEmail, string inputPassword, int? RememberMe)
        {
            var user = db.Users.SingleOrDefault(m => m.email == InputEmail && m.password == inputPassword);
            if (user != null)
            {
                Session["user"] = user;

                if (RememberMe == 1)
                {
                    HttpCookie cookie = new HttpCookie("LoginCookie");
                    cookie.Values["Email"] = InputEmail;
                    cookie.Values["Password"] = inputPassword;
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Add(cookie);
                }

                return Redirect("~/Home/Index?loginError=false");
            }
            else
            {
                return Redirect("~/Home/Index?loginError=true");
            }
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "user_id,username,password,email,user_avatar,user_background,created_at")] User user, string rePass)
        {
            // Đặt giá trị mặc định
            user.user_avatar = null;
            user.user_background = null;
            user.created_at = DateTime.Now;

            // Kiểm tra có trùng email người dùng khác không
            var userdb = db.Users.SingleOrDefault(m => m.email == user.email);

            if (ModelState.IsValid && user.password == rePass && userdb == null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return Redirect("~/Home/Index?registerError=false");
            }
            return Redirect("~/Home/Index?registerError=true");
        }

        public ActionResult LogOut()
        {
            // Xóa Session
            Session.Clear();

            // Xóa cookie
            if (Request.Cookies["LoginCookie"] != null)
            {
                var cookie = new HttpCookie("LoginCookie");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return Redirect("~/Home/Index");
        }
    }
}