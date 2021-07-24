using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;

namespace Ictshop.Areas.Admin.Controllers
{
    public class DonhangsController : Controller
    {
        private Qlbanhang db = new Qlbanhang();
        private SessionController session = new SessionController();

        // GET: Admin/Donhangs
        public ActionResult Index()
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var donhangs = db.Donhangs.Include(d => d.Nguoidung).OrderByDescending(n => n.Madon);
            return View(donhangs.ToList());
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }

            var donhangs = db.Chitietdonhangs.Where(c => c.Madon == id)
                                            .ToList();
            return View(donhangs);
        }

        // GET: Admin/Donhangs/Details/5
        public ActionResult Details(int? id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }

        // GET: Admin/Donhangs/Create
        public ActionResult Create()
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            ViewBag.MaNguoidung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten");
            return View();
        }

        // POST: Admin/Donhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Madon,Ngaydat,Tinhtrang,MaNguoidung")] Donhang donhang)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (ModelState.IsValid)
            {
                db.Donhangs.Add(donhang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoidung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten", donhang.MaNguoidung);
            return View(donhang);
        }

        // GET: Admin/Donhangs/Edit/5
        public ActionResult Edit(int? id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoidung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten", donhang.MaNguoidung);
            return View(donhang);
        }

        // POST: Admin/Donhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Madon,Ngaydat,Tinhtrang,MaNguoidung")] Donhang donhang)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (ModelState.IsValid)
            {
                donhang.MaDonMoMo = "DONHANGTRUCTIEP";
                db.Entry(donhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoidung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten", donhang.MaNguoidung);
            return View(donhang);
        }

        // GET: Admin/Donhangs/Delete/5
        public ActionResult Delete(int? id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }

        // POST: Admin/Donhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            Donhang donhang = db.Donhangs.Find(id);
            db.Donhangs.Remove(donhang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
