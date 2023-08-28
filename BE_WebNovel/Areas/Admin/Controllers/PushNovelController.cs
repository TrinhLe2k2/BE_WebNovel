using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class PushNovelController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Admin/PushNovel
        public ActionResult Index()
        {
            ViewBag.ListCategories = db.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "book_id,user_id,book_title,book_author,book_description,book_poster,book_created_at,book_status")] Book book, int[] CategoriesList, HttpPostedFileBase Poster)
        {
            var CurrentUser = (BE_WebNovel.Models.User)HttpContext.Session["user"];
            book.user_id = CurrentUser.user_id;
            book.book_created_at = DateTime.Now;
            if (ModelState.IsValid)
            {

                #region Xử lý ảnh
                if (Poster != null && Poster.ContentLength > 0)
                {
                    string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    // Lưu ảnh
                    var fileName = formattedDate  + "_Poster_" + Path.GetFileName(Poster.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/assets/img/Poster/"), fileName);
                    Poster.SaveAs(imagePath);
                    book.book_poster = fileName;
                }
                #endregion

                #region Thêm thể loại book
                if (CategoriesList != null)
                {
                    foreach (var item in CategoriesList)
                    {
                        BookCategory bookCategory = new BookCategory();
                        bookCategory.book_id = book.book_id;
                        bookCategory.category_id = item;
                        bookCategory.category_note = book.book_title;
                        db.BookCategories.Add(bookCategory);
                    }
                }
                
                #endregion

                #region khởi tạo mặc định BlackBook
                BlackBook blackBook = new BlackBook();
                blackBook.bookId = book.book_id;
                blackBook.note = "Truyện mới";
                blackBook.created_at = DateTime.Now;
                db.BlackBooks.Add(blackBook);
                #endregion

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListCategories = db.Categories.ToList();
            return View();
        }
    }
}