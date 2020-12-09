using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Shop___DAW.Models;

namespace Online_Shop___DAW.Controllers
{
    public class HomeController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        public ActionResult Index()
        {
            var categories = from cat in db.Categories orderby cat.Name ascending select cat;
            var dealsOfTheDay = from prod
                                in db.Products
                                where prod.DiscountedPrice != null
                                    && prod.Status == ProductStatus.accepted
                                select prod;
            ViewBag.Categories = categories.ToList();
            ViewBag.DealsOfTheDay = dealsOfTheDay
                .Take(10)
                .OrderByDescending(p => p.Price - p.DiscountedPrice)
                .ToList();
            return View();
        }
    }
}
