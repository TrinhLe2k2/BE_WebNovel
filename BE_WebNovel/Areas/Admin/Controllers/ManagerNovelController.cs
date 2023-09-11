using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using PagedList;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class ManagerNovelController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();

        [AdminAuthorize]
        public ActionResult Index(int? page)
        {
            if(page == null) page = 1;
            // truy van list
            var bookList = db.Books.ToList();
            int pageSize = 10;
            
            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];
            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.CountNovel = bookList.Count();

            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            int pageNumber = (page ?? 1);
            return View(bookList.ToPagedList(pageNumber, pageSize));
        }

        [AdminAuthorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];
            ViewBag.ListCategories = db.Categories.ToList();
            ViewBag.ListStatusBook = db.StatusBooks.ToList();

            // Toast
            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            // Error Edit form
            ViewBag.ErrorBookTitle = TempData["ErrorBookTitle"];

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,user_id,status_id,book_title,book_author,book_description,book_poster,book_created_at")] Book book, HttpPostedFileBase Poster, string originPoster, DateTime? time)
        {
            #region Đặt lại giá trị ban đầu
            book.book_created_at = time;
            #endregion

            if (ModelState.IsValid)
            {
                #region Xử lý ảnh
                if (Poster != null && Poster.ContentLength > 0)
                {
                    // Xóa ảnh
                    if (originPoster != "")
                    {
                        var relativeImagePath = "~/Content/assets/img/Poster/" + originPoster;
                        var serverPath = Server.MapPath(relativeImagePath);
                        System.IO.File.Delete(serverPath);
                    }
                    // Lưu ảnh
                    string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var fileName = formattedDate  + "_Poster_" + book.user_id + "_" + Path.GetFileName(Poster.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/assets/img/Poster/"), fileName);
                    Poster.SaveAs(imagePath);
                    book.book_poster = fileName;
                }
                else
                {
                    book.book_poster = originPoster;
                }
                #endregion

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                Toast(false, "Chỉnh sửa thành công");
                return Redirect("~/Admin/CreatedNovel/Index");
            }
            if(book.book_title == null)
            {
                TempData["ErrorBookTitle"] = "Vui lòng nhập vào trường này";
            }
            Toast(true, "Chỉnh sửa thất bại");
            return Redirect("~/Admin/ManagerNovel/Edit/"+book.book_id);
        }
    
        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var currentBook = db.Books.Find(id);
            if (currentBook.book_poster != null)
            {
                var relativeImagePath = "~/Content/assets/img/Poster/" + currentBook.book_poster;
                var serverPath = Server.MapPath(relativeImagePath);
                System.IO.File.Delete(serverPath);
            }

            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            Toast(false, "Đã xóa thành công");
            return RedirectToAction("Index");
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBackBook([Bind(Include = "blackId,bookId,note,created_at")] BlackBook blackBook)
        {
            blackBook.created_at = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.BlackBooks.Add(blackBook);
                db.SaveChanges();
                Toast(true, "Truyện đã được khóa");
                return RedirectToAction("Index");
            }
            ViewBag.bookId = new SelectList(db.Books, "book_id", "book_title", blackBook.bookId);
            return View(blackBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBackBook(int bookID)
        {
            BlackBook blackBook = db.BlackBooks.SingleOrDefault(b=>b.bookId == bookID);
            db.BlackBooks.Remove(blackBook);
            db.SaveChanges();
            Toast(false, "Truyện đã được duyệt thành công");
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