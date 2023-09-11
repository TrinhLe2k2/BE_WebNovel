using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class ManagerChapterController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Admin/ManagerChapter
        [AdminAuthorize]
        public ActionResult Index(int bookID)
        {
            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];
            ViewBag.CurrentBook = db.Books.SingleOrDefault(b=>b.book_id == bookID);
            var CountChapters = db.Chapters.Where(c=>c.book_id == bookID).Count();
            ViewBag.CountChapters = CountChapters;

            if(CountChapters == 0)
            {
                ViewBag.NewChapterNumber = 1;
            }
            else
            {
                ViewBag.NewChapterNumber = db.Chapters
                    .Where(c => c.book_id == bookID)
                    .OrderByDescending(c => c.chapter_number)
                    .FirstOrDefault().chapter_number + 1;
            }

            #region Notification
            // Toast
            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            // Error Create
            if(TempData["ErrorCreate"] != null)
            {
                ViewBag.ErrorCreate = TempData["ErrorCreate"];
                if(ViewBag.ErrorCreate == true)
                {
                    ViewBag.chapter_number = TempData["chapter_number"];
                    ViewBag.chapter_title = TempData["chapter_title"];
                    ViewBag.BorderDangerForChapterNumber = TempData["BorderDangerForChapterNumber"];
                    ViewBag.BorderDangerForChapterTitle = TempData["BorderDangerForChapterTitle"];
                }
            }

            #endregion


            return View();
        }
        [AdminAuthorize]
        public ActionResult RenderChapters(int bookID)
        {
            var Chapters = db.Chapters.Where(c=>c.book_id == bookID).OrderBy(c=>c.chapter_number).ToList();
            var formatChapters = Chapters.Select(chapter => new { 
                    chapter_id = chapter.chapter_id,
                    book_id = chapter.book_id,
                    chapter_title = chapter.chapter_title,
                    chapter_number = chapter.chapter_number,
                    chapter_created_at = chapter.chapter_created_at.Value.ToString("dd-MM-yyyy"),
                    chapter_view = chapter.chapter_view,
                    Comments = chapter.Comments,

                });
            return Json(formatChapters, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult Create([Bind(Include = "chapter_id,book_id,chapter_number,chapter_title,chapter_content,chapter_view,chapter_created_at")] Chapter chapter)
        {
            chapter.chapter_view = 0;
            chapter.chapter_created_at = DateTime.Now;
            if (ModelState.IsValid && chapter.chapter_number != null && chapter.chapter_title != null && chapter.chapter_number >= 0)
            {
                db.Chapters.Add(chapter);
                db.SaveChanges();
                Toast(false, "Đã thêm chương thành công");
                return Redirect("~/Admin/ManagerChapter/Index?bookID="+chapter.book_id);
            }

            TempData["ErrorCreate"] = true;
            Toast(true, "Thêm chương mới thất bại");
            if(chapter.chapter_number == null)
            {
                TempData["BorderDangerForChapterNumber"] = "border-danger";
                TempData["chapter_number"] = "Trường này không được để trống";
            } else if(chapter.chapter_number < 0) {
                TempData["BorderDangerForChapterNumber"] = "border-danger";
                TempData["chapter_number"] = "Vui lòng nhập số chương lớn hơn hoặc bằng 0";
            }
            if(chapter.chapter_title == null)
            {
                TempData["BorderDangerForChapterTitle"] = "border-danger";
                TempData["chapter_title"] = "Trường này không được để trống";
            }
            var bookID = chapter.book_id;
            return Redirect("~/Admin/ManagerChapter/Index?bookID="+bookID);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];
            #region Notification
            // Toast
            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            // Error Edit
            if(TempData["ErrorEdit"] != null)
            {
                ViewBag.ErrorEdit = TempData["ErrorEdit"];
                if(ViewBag.ErrorEdit == true)
                {
                    ViewBag.chapter_number = TempData["chapter_number"];
                    ViewBag.chapter_title = TempData["chapter_title"];
                    ViewBag.BorderDangerForChapterNumber = TempData["BorderDangerForChapterNumber"];
                    ViewBag.BorderDangerForChapterTitle = TempData["BorderDangerForChapterTitle"];
                }
            }
            #endregion
            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chapter_id,book_id,chapter_number,chapter_title,chapter_content,chapter_view,chapter_created_at")] Chapter chapter)
        {
            if (ModelState.IsValid && chapter.chapter_number != null && chapter.chapter_title != null && chapter.chapter_number >= 0)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                Toast(false, "Cập nhật thành công");
                return Redirect("~/Admin/ManagerChapter/Index?bookID="+chapter.book_id);
            }
            TempData["ErrorEdit"] = true;
            Toast(true, "Cập nhật thất bại");
            if(chapter.chapter_number == null)
            {
                TempData["BorderDangerForChapterNumber"] = "border-danger";
                TempData["chapter_number"] = "Trường này không được để trống";
            } else if(chapter.chapter_number < 0) {
                TempData["BorderDangerForChapterNumber"] = "border-danger";
                TempData["chapter_number"] = "Vui lòng nhập số chương lớn hơn hoặc bằng 0";
            }
            if(chapter.chapter_title == null)
            {
                TempData["BorderDangerForChapterTitle"] = "border-danger";
                TempData["chapter_title"] = "Trường này không được để trống";
            }
            return Redirect("~/Admin/ManagerChapter/Edit/" + chapter.chapter_id);
            //return View(chapter);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            var bookID = chapter.book_id;
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            Toast(false, "Đã xóa thành công");
            return Redirect("~/Admin/ManagerChapter/Index?bookID="+bookID);
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