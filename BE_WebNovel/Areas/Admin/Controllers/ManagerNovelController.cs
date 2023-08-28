using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BE_WebNovel.Models;
using PagedList;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class ManagerNovelController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();
        // GET: Admin/ManagerNovel
        public ActionResult Index(int? page)
        {
            if(page == null) page = 1;
            // truy van list
            var bookList = db.Books.ToList();
            int pageSize = 1;
            
            int pageNumber = (page ?? 1);
            return View(bookList.ToPagedList(pageNumber, pageSize));
        }
    }
}