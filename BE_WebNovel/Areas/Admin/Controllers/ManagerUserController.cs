using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class ManagerUserController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Admin/ManagerUser
        [AdminAuthorize]
        public ActionResult Index(int? page)
        {
            if(page == null) page = 1;
            // truy van list
            var userList = db.Users.ToList();
            int pageSize = 10;
            
            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.CountUser = db.Users.Count();

            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];

            #region Đăng ký lỗi
            ViewBag.registerError = TempData["registerError"];
            ViewBag.ErrorUsername = TempData["ErrorUsername"];
            ViewBag.ErrorEmail = TempData["ErrorEmail"];
            ViewBag.ErrorPassword = TempData["ErrorPassword"];
            ViewBag.ErrorRePassword = TempData["ErrorRePassword"];
            ViewBag.userInput = TempData["userInput"];
            #endregion

            ViewBag.Permissions = db.permissions.ToList();

            int pageNumber = (page ?? 1);
            return View(userList.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult create([Bind(Include = "user_id,permission_id,username,password,email,user_avatar,user_background,created_at")] User user, string rePass)
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
                Toast(false, "Thêm tài khoản thành công");
                return RedirectToAction("Index");
            }

            TempData["registerError"] = true;
            Toast(true, "Thêm tài khoản thất bại");

            if(user.username == null)
            {
                TempData["ErrorUsername"] = "Vui lòng nhập trường này";
            }
            if(user.email == null)
            {
                TempData["ErrorEmail"] = "Vui lòng nhập trường này";
            }
            else if(userdb != null)
            {
                TempData["ErrorEmail"] = "Email đã tồn tại";
            }
            if(user.password == null)
            {
                TempData["ErrorPassword"] = "Vui lòng nhập trường này";
            }
            if(rePass == null)
            {
                TempData["ErrorRePassword"] = "Vui lòng nhập trường này";
            }
            else if(user.password != rePass)
            {
                TempData["ErrorRePassword"] = "Mật khẩu xác nhận không chính xác";
            }
            TempData["userInput"] = user;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult Delete(int? id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            Toast(false, "Đã xóa thành công");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult AddBlackUser([Bind(Include = "blackId,userId,note,created_at")] BlackUser blackUser)
        {
            blackUser.created_at = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.BlackUsers.Add(blackUser);
                db.SaveChanges();
                Toast(false, "Đã khóa thành công");
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.Users, "user_id", "username", blackUser.userId);
            return View(blackUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBlackUser(int userID)
        {
            BlackUser blackUser = db.BlackUsers.SingleOrDefault(b=>b.userId == userID);
            db.BlackUsers.Remove(blackUser);
            db.SaveChanges();
            Toast(false, "Đã mở khóa thành công");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult permissionForUser(int userID, int permission)
        {
            var EditUser = db.Users.Find(userID);
            EditUser.permission_id = permission;
            db.Entry(EditUser).State = EntityState.Modified;
            db.SaveChanges();
            Toast(false, "Đã thay đổi quyền thành công");
            return RedirectToAction("Index");
        }

        public void Toast(bool isError, string contentToast)
        {
            if(isError)
            {
                TempData["isError"] = true;
                TempData["Color"] = "#dc3545";
                TempData["ToastContent"] = contentToast;
            }
            else
            {
                TempData["isError"] = false;
                TempData["Color"] = "#00dc82";
                TempData["ToastContent"] = contentToast;
            }
        }
    }
}