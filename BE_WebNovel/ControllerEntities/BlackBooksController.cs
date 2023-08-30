using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BE_WebNovel.Models;

namespace BE_WebNovel.ControllerEntities
{
    public class BlackBooksController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();

        // GET: BlackBooks
        public ActionResult Index()
        {
            var blackBooks = db.BlackBooks.Include(b => b.Book);
            return View(blackBooks.ToList());
        }

        // GET: BlackBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlackBook blackBook = db.BlackBooks.Find(id);
            if (blackBook == null)
            {
                return HttpNotFound();
            }
            return View(blackBook);
        }

        // GET: BlackBooks/Create
        public ActionResult Create()
        {
            ViewBag.bookId = new SelectList(db.Books, "book_id", "book_title");
            return View();
        }

        // POST: BlackBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "blackId,bookId,note,created_at")] BlackBook blackBook)
        {
            if (ModelState.IsValid)
            {
                db.BlackBooks.Add(blackBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bookId = new SelectList(db.Books, "book_id", "book_title", blackBook.bookId);
            return View(blackBook);
        }

        // GET: BlackBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlackBook blackBook = db.BlackBooks.Find(id);
            if (blackBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.bookId = new SelectList(db.Books, "book_id", "book_title", blackBook.bookId);
            return View(blackBook);
        }

        // POST: BlackBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "blackId,bookId,note,created_at")] BlackBook blackBook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blackBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bookId = new SelectList(db.Books, "book_id", "book_title", blackBook.bookId);
            return View(blackBook);
        }

        // GET: BlackBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlackBook blackBook = db.BlackBooks.Find(id);
            if (blackBook == null)
            {
                return HttpNotFound();
            }
            return View(blackBook);
        }

        // POST: BlackBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlackBook blackBook = db.BlackBooks.Find(id);
            db.BlackBooks.Remove(blackBook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
