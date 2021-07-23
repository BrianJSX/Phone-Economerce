using Ictshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Ictshop.Controllers
{
    public class HomeController : Controller
    {
        Qlbanhang db = new Qlbanhang();

        public ActionResult Search( string key, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.key = key;
            var sanpham = db.Sanphams.SqlQuery("select * from SanPham where Tensp like '%"+ key +"%' ORDER BY Masp ");
            return View(sanpham.ToPagedList(pageNumber, pageSize));
        }

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