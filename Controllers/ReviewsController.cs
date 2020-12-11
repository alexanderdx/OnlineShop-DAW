﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;

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
        public ActionResult New(Review review)
        {
            try
            {
                review.CreatedAt = DateTime.UtcNow;
                db.Reviews.Add(review);
                db.SaveChanges();
                return Redirect("/products/show/" + review.ProductId);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            var review = db.Reviews
                .Include("Product")
                .FirstOrDefault(p => p.ReviewId == id);
            ViewBag.Review = review;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                Review review = db.Reviews.Find(id);
                if (TryUpdateModel(review))
                {
                    review.Title = requestReview.Title;
                    review.Content = requestReview.Content;
                    review.Rating = requestReview.Rating;
                    db.SaveChanges();
                }
                return Redirect("/products/show/" + review.ProductId);
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
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return Redirect("/products/show/" + review.ProductId);
        }
    }
}