using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Controllers
{
    public class HomeController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Home
        public ActionResult Index(bool? loginError, bool? registerError)
        {
            #region Login
            if (loginError!=null)
            {
                if(loginError == true)
                {
                    ViewBag.loginError = true;
                    ViewBag.Color = "#dc3545";
                    ViewBag.ToastContent = "Đăng nhập không thành công";
                    ViewBag.LoginErrorMessage = "Email hoặc mật khẩu không chính xác";
                }
                else
                {
                    ViewBag.Color = "#00dc82";
                    ViewBag.ToastContent = "Đăng nhập thành công";
                }
            }
            #endregion

            #region Register
            if (registerError != null)
            {
                if (registerError == true)
                {
                    ViewBag.registerError = true;
                    ViewBag.Color = "#dc3545";
                    ViewBag.ToastContent = "Đăng ký không thành công";
                    ViewBag.EmailerrorMessage  = "Email đã tồn tại hoặc không chính xác";
                } else
                {
                    ViewBag.Color = "#00dc82";
                    ViewBag.ToastContent = "Đăng ký thành công";
                }
            }
            #endregion

            #region Check cookie
            if (Request.Cookies["LoginCookie"] != null)
            {
                string email = Request.Cookies["LoginCookie"]["Email"];
                string password = Request.Cookies["LoginCookie"]["Password"];

                var user = db.Users.SingleOrDefault(m => m.email == email && m.password == password);

                if (user != null)
                {
                    Session["user"] = user;
                    ViewBag.Color = "#00dc82";
                    ViewBag.ToastContent = "Đăng nhập thành công";
                    return View();
                }
            }
            #endregion
            
            if(loginError == null && registerError == null)
            {
                ViewBag.Color = "blue";
                ViewBag.ToastContent = "Chào mừng bạn đến với bình nguyên vô tận";
            }
            return View();
        }
    }
}