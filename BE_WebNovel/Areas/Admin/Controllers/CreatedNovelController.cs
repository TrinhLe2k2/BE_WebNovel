using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class CreatedNovelController : Controller
    {
        // GET: Admin/CreatedNovel
        public ActionResult Index()
        {
            return View();
        }
    }
}