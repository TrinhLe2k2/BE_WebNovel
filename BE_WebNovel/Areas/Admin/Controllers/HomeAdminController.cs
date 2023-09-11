using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Admin/HomeAdmin
        [AdminAuthorize]
        public ActionResult Index()
        {
            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];

            var CurrentUser = (BE_WebNovel.Models.User)HttpContext.Session["user"];
            var userDB = db.Users.FirstOrDefault(f => f.user_id == CurrentUser.user_id);
            return View(userDB);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "user_id,permission_id,username,password,email,user_avatar,user_background,created_at")] User user, HttpPostedFileBase avatar, HttpPostedFileBase background, string NewPassword, string RePassword)
        {
            var CurrentUser = (BE_WebNovel.Models.User)HttpContext.Session["user"];

            // Cập nhật giá trị mặc định cho user
            user.created_at = CurrentUser.created_at;
            if (ModelState.IsValid && user.password == CurrentUser.password)
            {
                #region Xử lý ảnh
                //Lưu avatar
                if (avatar != null && avatar.ContentLength > 0)
                {
                    // Xóa ảnh
                    if (CurrentUser.user_avatar != null)
                    {
                        var relativeImagePath = "~/Content/assets/img/User/Avatar/" + CurrentUser.user_avatar;
                        var serverPath = Server.MapPath(relativeImagePath);
                        System.IO.File.Delete(serverPath);
                    }
                    // Lưu ảnh
                    var fileName = CurrentUser.user_id + "_Avatar_" + Path.GetFileName(avatar.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/assets/img/User/Avatar/"), fileName);
                    avatar.SaveAs(imagePath);
                    user.user_avatar = fileName;
                }
                else
                {
                    user.user_avatar = CurrentUser.user_avatar;
                }


                // Lưu background
                if (background != null && background.ContentLength > 0)
                {
                    // Xóa ảnh
                    if (CurrentUser.user_background != null)
                    {
                        var relativeImagePath = "~/Content/assets/img/User/Background/" + CurrentUser.user_background;
                        var serverPath = Server.MapPath(relativeImagePath);
                        System.IO.File.Delete(serverPath);
                    }
                    // Lưu ảnh
                    var fileName = CurrentUser.user_id + "_Background_" + Path.GetFileName(background.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/assets/img/User/Background/"), fileName);
                    background.SaveAs(imagePath);
                    user.user_background = fileName;
                }
                else
                {
                    user.user_background = CurrentUser.user_background;
                }
                #endregion

                #region Xử lý khi thay đổi mật khẩu
                //Lưu password , xóa session và xóa cookie
                if (NewPassword != "" && RePassword == NewPassword)
                {
                    user.password = NewPassword;
                    Session.Clear();
                    if (Request.Cookies["LoginCookie"] != null)
                    {
                        var cookie = new HttpCookie("LoginCookie");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie);
                    }
                    // Cập nhật session
                    var UpdateUser = db.Users.SingleOrDefault(u=>u.user_id == user.user_id);
                    Session["user"] = UpdateUser;
                }
                #endregion
                // Lưu db
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                //return Redirect("~/Admin/HomeAdmin/Index");
                Session["user"] = user;
                ToastErrorUpdateAccount(false);
                return View(user);
            }
            else
            {
                #region Xử lý thông báo

                // Xử lý thông báo lỗi về input
                if (RePassword != NewPassword && NewPassword != "")
                {
                    ViewBag.MessRePass = "Mật khẩu xác nhận không chính xác";
                }

                if(RePassword != NewPassword && NewPassword == "")
                {
                    ViewBag.MessNewPass = "Vui lòng nhập trường này";
                }
                if(user.password != CurrentUser.password)
                {
                    ViewBag.MessPass = "Vui lòng nhập trường này";
                    ViewBag.Border = "border: solid 2px #dc3545";
                }
                #endregion
                ToastErrorUpdateAccount(true);
                return View(CurrentUser);
            }
            
        }

        void ToastErrorUpdateAccount(bool isError)
        {
            if(isError)
            {
                ViewBag.isError = true;
                ViewBag.Color = "#dc3545";
                ViewBag.ToastContent = "Cập nhật thất bại";
            }
            else
            {
                ViewBag.isError = false;
                ViewBag.Color = "#00dc82";
                ViewBag.ToastContent = "Cập nhật thành công";
            }
        }
    }
}