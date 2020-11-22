using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop___DAW.Controllers
{
    public class OrdersController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }
    }
}