using BE_WebNovel.App_Start;
using BE_WebNovel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_WebNovel.Areas.Admin.Controllers
{
    public class ManagerCategoryController : Controller
    {
        private WebNovelEntities db = new WebNovelEntities();

        [AdminAuthorize]
        public ActionResult Index()
        {
            ViewBag.isError = TempData["isError"];
            ViewBag.Color = TempData["Color"];
            ViewBag.ToastContent = TempData["ToastContent"];

            ViewBag.CountCategories = db.Categories.Count();

            return View();
        }
        public ActionResult RenderCategoriesList()
        {
            var CategoriesList = db.Categories.ToList();

            var formattedCategoriesList = CategoriesList.Select(category => new
            {
                category_id = category.category_id,
                category_name = category.category_name,
                category_created_at = category.category_created_at.Value.ToString("yyyy-MM-dd") // Định dạng ngày tháng theo mong muốn
            });;

            return Json(formattedCategoriesList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "category_id,category_name,category_created_at")] Category category)
        {
            var existCategory = db.Categories.SingleOrDefault(m => m.category_name == category.category_name);
            category.category_created_at = DateTime.Now;
            if (ModelState.IsValid && existCategory == null)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                ErrorToast(false, "Thêm thành công");
                return RedirectToAction("Index");
            }
            ErrorToast(true, "Thất bại: Thể loại đã tồn tại");
            return RedirectToAction("Index");
        }

        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            ErrorToast(false, "Xóa thành công");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResultSearchCategory(string stringSearch)
        {
            var CategoriesList = db.Categories.ToList();
            return Json(CategoriesList, JsonRequestBehavior.AllowGet);
        }

        public void ErrorToast(bool isError, string contentToast)
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