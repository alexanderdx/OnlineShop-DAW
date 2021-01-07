using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopDAW.Models;
using Microsoft.AspNet.Identity;

namespace OnlineShopDAW.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["cart"] == null)
            {
                ViewBag.EmptyCart = "Cosul dvs. este gol!";
            }
            else
            {
                var cart = Session["cart"];
                ViewBag.CartItems = cart;
            }

            return View();
        }

        public ActionResult AddToCart(int id)
        {
            Product product = db.Products.Find(id);
            var cart = new List<Tuple<Product, int>>();

            if (Session["cart"] == null)
            {
                cart.Add(new Tuple<Product, int>(product, 1));
            }
            else
            {
                cart = (List<Tuple<Product, int>>)Session["cart"];
                int index = itemAlreadyInCart(id);
                if (index != -1)
                {
                    int quantity = cart[index].Item2;
                    cart[index] = new Tuple<Product, int>(product, quantity + 1);
                }
                else
                {
                    cart.Add(new Tuple<Product, int>(product, 1));
                }
            }
            
            Session["cart"] = cart;
            ViewBag.CartItems = cart;
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Remove(int id)
        {
            var cart = (List<Tuple<Product, int>>)Session["cart"];
            int index = itemAlreadyInCart(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }

        private int itemAlreadyInCart(int id)
        {
            var cart = (List<Tuple<Product, int>>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Item1.ProductId.Equals(id))
                    return i;

            return -1;
        }

    }
}
