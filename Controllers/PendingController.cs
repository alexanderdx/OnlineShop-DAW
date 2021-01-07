using OnlineShopDAW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopDAW.Controllers
{
    [Authorize(Roles = "administrator")]
    public class PendingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pending
        public ActionResult Index()
        {
            var products = from prod
                           in db.Products
                           select prod;

            ViewBag.Products = products
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View();
        }


        public ActionResult Accept(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (TryUpdateModel(product))
                {
                    product.Status = ProductStatus.accepted;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

            return RedirectToAction("Index", "Pending");
        }

        public ActionResult Reject(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (TryUpdateModel(product))
                {
                    product.Status = ProductStatus.rejected;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

            return RedirectToAction("Index", "Pending");
        }

        public ActionResult Postpone(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (TryUpdateModel(product))
                {
                    product.Status = ProductStatus.pending;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

            return RedirectToAction("Index", "Pending");
        }
    }
}