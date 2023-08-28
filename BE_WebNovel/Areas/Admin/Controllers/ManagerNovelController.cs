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
            
            int pageNumber = (page ?? 1);
            return View(bookList.ToPagedList(pageNumber, pageSize));
        }

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

            ViewBag.ListCategories = db.Categories.ToList();
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", book.user_id);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,user_id,book_title,book_author,book_description,book_poster,book_created_at,book_status")] Book book, HttpPostedFileBase Poster, string originPoster, DateTime time)
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
                return Redirect("~/Admin/CreatedNovel/Index");
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "username", book.user_id);
            return View(book);
        }
    }
}