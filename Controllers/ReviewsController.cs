using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;
using Microsoft.AspNet.Identity;

namespace OnlineShopDAW.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(int id)
        {
            var review = db.Reviews
                .Include("Product")
                .FirstOrDefault(p => p.ReviewId == id);
            ViewBag.Review = review;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "administrator,collaborator,registered")]
        public ActionResult New(Review review)
        {
            try
            {
                review.CreatedAt = DateTime.UtcNow;
                review.Product = db.Products.First(p => p.ProductId == review.Product.ProductId);
                review.ApplicationUser = db.Users.First(u => u.Id == User.Identity.GetUserId());
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Show", "Products", new { id = review.Product.ProductId });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize(Roles = "administrator,collaborator,registered")]
        public ActionResult Edit(int id)
        {
            var review = db.Reviews
                .Include("Product")
                .FirstOrDefault(p => p.ReviewId == id);

            if (review.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
            {
                ViewBag.Review = review;
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati acest review!";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPut]
        [Authorize(Roles = "administrator,collaborator,registered")]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                Review review = db.Reviews.Find(id);

                if (review.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
                {
                    if (TryUpdateModel(review))
                    {
                        review.Title = requestReview.Title;
                        review.Content = requestReview.Content;
                        review.Rating = requestReview.Rating;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Show", "Products", new { id = review.Product.ProductId });
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa modificati acest review!";
                    return RedirectToAction("Index", "Products");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "administrator,collaborator,registered")]
        public ActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review.ApplicationUser.Id == User.Identity.GetUserId() || User.IsInRole("administrator"))
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
                return RedirectToAction("Show", "Products", new { id = review.Product.ProductId });
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati acest review!";
                return RedirectToAction("Index", "Products");
            }
        }
    }
}