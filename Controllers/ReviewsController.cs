using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop___DAW.Controllers
{
    public class ReviewsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }
    }
}