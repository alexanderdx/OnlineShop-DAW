﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop___DAW.Controllers
{
    public class ProductsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
    }
}