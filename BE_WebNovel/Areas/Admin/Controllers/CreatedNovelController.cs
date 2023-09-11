using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class CreatedNovelController : Controller
    {
         private WebNovelEntities db = new WebNovelEntities();
        [AdminAuthorize]
        public ActionResult Index(int? page)
        {
            var currentUser = (User)HttpContext.Session["user"];
            ViewBag.CurrtentUser = (User)HttpContext.Session["user"];
            if(page == null) page = 1;
            // truy van list
            var bookList = db.Books.Where(b=>b.user_id == currentUser.user_id).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;

            ViewBag.CountCreatedNovel = bookList.Count();

            // Toast
            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];
            return View(bookList.ToPagedList(pageNumber, pageSize));
        }
    }
}