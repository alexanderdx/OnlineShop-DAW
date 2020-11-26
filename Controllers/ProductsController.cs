using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Shop___DAW.Models;

namespace Online_Shop___DAW.Controllers
{
    public class ProductsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Products
        public ActionResult Index()
        {
            var products = from prod
                           in db.Products.Include("Category").Include("Reviews")
                           select prod;
            ViewBag.Products = products;
            return View();
        }

        public ActionResult Show(int id)
        {
            var product = db.Products
                .Include("Category")
                .Include("Reviews")
                .FirstOrDefault(p => p.ProductId == id);
            ViewBag.Product = product;
            return View(product);
        }

        public ActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();

            return View(product);
        }

        [HttpPost]
        public ActionResult New(Product product)
        {
            try
            {
                product.CreatedAt = DateTime.UtcNow;
                product.Categ = GetAllCategories();
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost trimis catre evaluare! Va multumim!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            var product = db.Products
                .Include("Category")
                .Include("Reviews")
                .FirstOrDefault(p => p.ProductId == id);
            product.Categ = GetAllCategories();
            ViewBag.Product = product;
            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int id, Product requestProduct)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (TryUpdateModel(product))
                {
                    product.Name = requestProduct.Name;
                    product.Description = requestProduct.Description;
                    product.Price = requestProduct.Price;
                    product.DiscountedPrice = requestProduct.DiscountedPrice;
                    product.Image = requestProduct.Image;
                    product.Stock = requestProduct.Stock;
                    product.Status = requestProduct.Status;
                    product.Category = requestProduct.Category;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categories = from cat in db.Categories
                             select cat;

            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }

            return selectList;
        }
    }
}
