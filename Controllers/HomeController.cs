using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;

namespace OnlineShopDAW.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var categories = from cat in db.Categories.Include("Products")
                             orderby cat.Name ascending
                             select cat;
            var dealsOfTheDay = from prod
                                in db.Products
                                where prod.DiscountedPrice != null
                                    && prod.Status == ProductStatus.accepted
                                select prod;
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
            ViewBag.DealsOfTheDay = dealsOfTheDay
                .Take(10)
                .OrderByDescending(p => p.Price - p.DiscountedPrice)
                .ToList();
            return View();
        }
    }
}
