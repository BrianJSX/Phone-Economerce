using Ictshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ictshop.Controllers
{
    public class HomeController : Controller
    {
        Qlbanhang db = new Qlbanhang();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SlidePartial()
        {
            return PartialView();

        }
    }
}