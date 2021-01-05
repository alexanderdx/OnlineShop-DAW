using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;

namespace OnlineShopDAW.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var categories = from category in db.Categories
                             orderby category.Name
                             select category;
            ViewBag.Categories = categories;
            ViewBag.arePermisiuni = User.IsInRole("administrator") || User.IsInRole("collaborator");
            return View();
        }

        public ActionResult Show(int id)
        {
            var category = db.Categories
                .Include("Products")
                .FirstOrDefault(p => p.CategoryId == id);
            ViewBag.Category = category;
            return View();
        }

        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "administrator,collaborator")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                category.CreatedAt = DateTime.UtcNow;
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult Edit(int id)
        {
            var category = db.Categories
                .Include("Products")
                .FirstOrDefault(p => p.CategoryId == id);
            ViewBag.Category = category;
            return View(category);
        }

        [HttpPut]
        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                }
                TempData["message"] = "Categoria a fost modificata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa!";
            return RedirectToAction("Index");
        }
    }
}