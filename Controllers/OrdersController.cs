using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;

namespace OnlineShopDAW.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(int id)
        {
            var order = db.Orders
                .Include("OrderProducts")
                .FirstOrDefault(p => p.OrderId == id);
            ViewBag.Order = order;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Order order)
        {
            try
            {
                order.CreatedAt = DateTime.UtcNow;
                db.Orders.Add(order);
                db.SaveChanges();
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
            var order = db.Orders
                .Include("OrderProducts")
                .FirstOrDefault(p => p.OrderId == id);
            ViewBag.Order = order;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Order requestOrder)
        {
            try
            {
                Order order = db.Orders.Find(id);
                if (TryUpdateModel(order))
                {
                    order.Status = requestOrder.Status;
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
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}