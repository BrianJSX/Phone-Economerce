using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;
using PagedList;

namespace Ictshop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        Qlbanhang db = new Qlbanhang();
        SessionController session = new SessionController();

        public ActionResult Index(int ?page)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }

            if (page == null) page = 1;
            var sp = db.Sanphams.OrderBy(x => x.Masp);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sp.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var dt = db.Sanphams.Find(id);
            return View(dt);
        }

        public ActionResult Create()
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var hangselected = new SelectList(db.Hangsanxuats, "Mahang", "Tenhang");
            ViewBag.Mahang = hangselected;
            var hdhselected = new SelectList(db.Hedieuhanhs, "Mahdh", "Tenhdh");
            ViewBag.Mahdh = hdhselected;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sanpham sanpham)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            try
            { 
                db.Sanphams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var dt = db.Sanphams.Find(id);
            var hangselected = new SelectList(db.Hangsanxuats, "Mahang", "Tenhang",dt.Mahang);
            ViewBag.Mahang = hangselected;
            var hdhselected = new SelectList(db.Hedieuhanhs, "Mahdh", "Tenhdh",dt.Mahdh);
            ViewBag.Mahdh = hdhselected;
            return View(dt);
        }

        [HttpPost]
        public ActionResult Edit(Sanpham sanpham)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }

            try
            {
                var oldItem = db.Sanphams.Find(sanpham.Masp);
                oldItem.Tensp = sanpham.Tensp;
                oldItem.Giatien = sanpham.Giatien;
                oldItem.Soluong = sanpham.Soluong;
                oldItem.Mota = sanpham.Mota;
                oldItem.Anhbia = sanpham.Anhbia;
                oldItem.Bonhotrong = sanpham.Bonhotrong;
                oldItem.Ram = sanpham.Ram;
                oldItem.Thesim = sanpham.Thesim;
                oldItem.Mahang = sanpham.Mahang;
                oldItem.Mahdh = sanpham.Mahdh;
                oldItem.dophangiai = sanpham.dophangiai;
                oldItem.chip = sanpham.chip;
                oldItem.manghinh = sanpham.manghinh;
                oldItem.cmrtruoc = sanpham.cmrtruoc;
                oldItem.cmrsau = sanpham.cmrsau;
                oldItem.pin = sanpham.pin;
                oldItem.giachuagiam = sanpham.giachuagiam;
                oldItem.xuatsu = sanpham.xuatsu;
                oldItem.ngayramat = sanpham.ngayramat;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var dt = db.Sanphams.Find(id);
            return View(dt);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            try
            {
                var dt = db.Sanphams.Find(id);
                db.Sanphams.Remove(dt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
