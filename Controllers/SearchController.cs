using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;

namespace OnlineShopDAW.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Index(
            string query,
            string skip,
            string take,
            string entity_type,
            string order_by
        )
        {
            if (query == null || query.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            int parsedSkip, parsedTake;
            try
            {
                parsedSkip = Int32.Parse(skip);
            }
            catch
            {
                parsedSkip = 0;
            }
            try
            {
                parsedTake = Int32.Parse(take);
            }
            catch
            {
                parsedTake = 10;
            }
            if (order_by != "price_asc" && order_by != "price_desc" && order_by != "name_desc")
            {
                ViewBag.OrderBy = "name_asc";
            }
            else
            {
                ViewBag.OrderBy = order_by;
            }

            if (entity_type != "category" && entity_type != "review")
            {
                entity_type = "product";
            }

            var categories = from cat in db.Categories.Include("Products")
                             orderby cat.Name ascending
                             select cat;
            ViewBag.Categories = categories.ToList();
            var rand = new Random();
            foreach (Category c in ViewBag.Categories)
            {
                c.Products = c.Products
                    .Where(p => p.Status == ProductStatus.accepted)
                    .OrderBy(x => rand.Next())
                    .Take(rand.Next(2, 6))
                    .ToList();
            }
            int totalEntities;
            if (entity_type == "review")
            {
                var reviewsFound = from rev in db.Reviews.Include("Product")
                                   where rev.Content.Contains(query) || rev.Title.Contains(query)
                                   select rev;
                totalEntities = reviewsFound.Count();
                if (order_by == "name_desc")
                {
                    ViewBag.EntitiesFound = reviewsFound
                        .OrderByDescending(x => x.Product.Name)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else if (order_by == "price_asc")
                {
                    ViewBag.EntitiesFound = reviewsFound
                        .OrderBy(x => x.Product.Price)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else if (order_by == "price_desc")
                {
                    ViewBag.EntitiesFound = reviewsFound
                        .OrderByDescending(x => x.Product.Price)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else
                {
                    ViewBag.EntitiesFound = reviewsFound
                        .OrderBy(x => x.Product.Name)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
            }
            else if (entity_type == "category")
            {
                var categoriesFound = from cat in db.Categories.Include("Products")
                                      where cat.Name.Contains(query) && cat.Products.Count > 0
                                      select cat;
                foreach (Category c in categoriesFound)
                {
                    c.Products = c.Products.Take(2).ToList();
                }
                totalEntities = categoriesFound.Count();
                ViewBag.EntitiesFound = categoriesFound
                    .OrderBy(x => x.Name)
                    .Skip(parsedSkip)
                    .Take(parsedTake)
                    .ToList();
            }
            else
            {
                var productsFound = from prod in db.Products
                                    where prod.Description.Contains(query)
                                        || prod.Name.Contains(query)
                                    select prod;
                totalEntities = productsFound.Count();
                if (order_by == "name_desc")
                {
                    ViewBag.EntitiesFound = productsFound
                        .OrderByDescending(x => x.Name)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else if (order_by == "price_asc")
                {
                    ViewBag.EntitiesFound = productsFound
                        .OrderBy(x => x.Price)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else if (order_by == "price_desc")
                {
                    ViewBag.EntitiesFound = productsFound
                        .OrderByDescending(x => x.Price)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
                else
                {
                    ViewBag.EntitiesFound = productsFound
                        .OrderBy(x => x.Name)
                        .Skip(parsedSkip)
                        .Take(parsedTake)
                        .ToList();
                }
            }
            ViewBag.Query = query;
            ViewBag.Skip = parsedSkip;
            ViewBag.Take = parsedTake;
            ViewBag.EntityType = entity_type;
            ViewBag.NumberOfPages = (int) Math.Ceiling((decimal) totalEntities / parsedTake);
            ViewBag.CurrentPage = 1 + (int) Math.Floor((decimal) parsedSkip / parsedTake);
            return View();
        }
    }
}