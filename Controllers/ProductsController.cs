using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;
using Microsoft.AspNet.Identity;

namespace OnlineShopDAW.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            var products = from prod
                           in db.Products.Include("Category").Include("Reviews")
                           select prod;

            ViewBag.esteAdmin = User.IsInRole("administrator");
            ViewBag.esteColaborator = User.IsInRole("collaborator");
            ViewBag.Products = products;
            return View();
        }

        public ActionResult Show(int id)
        {
            var product = db.Products
                .Include("Category")
                .Include("Reviews")
                .FirstOrDefault(p => p.ProductId == id);

            SetButtonVisibility(product);
            ViewBag.Product = product;
            return View(product);
        }

        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();

            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult New(Product product)
        {
            try
            {
                product.CreatedAt = DateTime.UtcNow;
                product.ApplicationUser = db.Users.First(u => u.Id == User.Identity.GetUserId());
                product.Category = db.Categories.First(c => c.CategoryId == product.Category.CategoryId);
                product.Categ = GetAllCategories();
                product.Status = ProductStatus.pending;

                TempData["message"] = "Produsul a fost trimis catre evaluare! Va multumim!";

                if (User.IsInRole("administrator"))
                {
                    TempData["message"] = "Produsul a fost adaugat!";
                    product.Status = ProductStatus.accepted;
                }

                db.Products.Add(product);
                db.SaveChanges();
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
            var product = db.Products
                .Include("Category")
                .Include("Reviews")
                .FirstOrDefault(p => p.ProductId == id);
            product.Categ = GetAllCategories();
            if (product.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
            {
                ViewBag.Product = product;
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati produse!";
                return Redirect("/products");
            }
        }

        [HttpPut]
        [Authorize(Roles = "administrator,collaborator")]
        public ActionResult Edit(int id, Product requestProduct)
        {
            try
            {
                Product product = db.Products.Find(id);

                if (product.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
                {
                    if (TryUpdateModel(product))
                    {
                        product.Name = requestProduct.Name;
                        product.Description = requestProduct.Description;
                        product.Price = requestProduct.Price;
                        product.DiscountedPrice = requestProduct.DiscountedPrice;
                        product.Image = requestProduct.Image;
                        product.Stock = requestProduct.Stock;
                        product.Status = requestProduct.Status;
                        product.Category = db.Categories
                            .First(c => c.CategoryId == requestProduct.Category.CategoryId);
                        product.Status = requestProduct.Status;
                        db.SaveChanges();
                        TempData["message"] = "Produsul a fost modificat!";
                    }
                    return Redirect("/products/show/" + product.ProductId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa modificati acest produs!";
                    return Redirect("/products");
                }
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
                Product product = db.Products.Find(id);
            if (product.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
            {

                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest produs!";
                return RedirectToAction("Index");
            }
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

        private void SetButtonVisibility(Product product)
        {
            ViewBag.afisareButoane = false;
            if (product.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("administrator");
            ViewBag.esteLogat = (User.IsInRole("administrator") || User.IsInRole("collaborator") || User.IsInRole("registered"));
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }
    }
}
