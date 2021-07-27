using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Areas.Admin.Controllers;
using Ictshop.Models;
using PagedList;


namespace Ictshop.Controllers
{
    public class DonHangsController : Controller
    {
        private SessionController session = new SessionController();
        private Qlbanhang db = new Qlbanhang();


        // GET: DonHangs
        public ActionResult Index(int? page)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (u == null)
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var donhangs = db.Donhangs.Where(n => n.MaNguoidung == u.MaNguoiDung)
                                        .Where(t => t.Tinhtrang != 3)
                                        .OrderByDescending(n => n.Madon);
            return View(donhangs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (u == null)
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }

            var donhangs = db.Chitietdonhangs.Where(c => c.Madon == id)
                                            .ToList();
            ViewBag.TongTien = 0;
            return View(donhangs);
        }
    }
}