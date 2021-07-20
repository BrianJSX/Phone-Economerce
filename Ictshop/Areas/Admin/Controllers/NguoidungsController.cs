﻿using System;
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
    public class NguoidungsController : Controller
    {
        private Qlbanhang db = new Qlbanhang();
        private SessionController session = new SessionController();

        public ActionResult Index()
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            var nguoidungs = db.Nguoidungs.Include(n => n.PhanQuyen);
            return View(nguoidungs.ToList());
        }

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
            Nguoidung nguoidung = db.Nguoidungs.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        //// GET: Admin/Nguoidungs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.IDQuyen = new SelectList(db.PhanQuyens, "IDQuyen", "TenQuyen");
        //    return View();
        //}

        //// POST: Admin/Nguoidungs/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaNguoiDung,Hoten,Email,Dienthoai,Matkhau,IDQuyen")] Nguoidung nguoidung)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Nguoidungs.Add(nguoidung);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.IDQuyen = new SelectList(db.PhanQuyens, "IDQuyen", "TenQuyen", nguoidung.IDQuyen);
        //    return View(nguoidung);
        //}


            // Chỉnh sửa người dùng
        // GET: Admin/Nguoidungs/Edit/5
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
            Nguoidung nguoidung = db.Nguoidungs.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDQuyen = new SelectList(db.PhanQuyens, "IDQuyen", "TenQuyen", nguoidung.IDQuyen);
            return View(nguoidung);
        }

        // POST: Admin/Nguoidungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNguoiDung,Hoten,Email,Dienthoai,Matkhau,IDQuyen")] Nguoidung nguoidung)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(nguoidung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDQuyen = new SelectList(db.PhanQuyens, "IDQuyen", "TenQuyen", nguoidung.IDQuyen);
            return View(nguoidung);
        }

        // Xoá người dùng 
        // GET: Admin/Nguoidungs/Delete/5
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
            Nguoidung nguoidung = db.Nguoidungs.Find(id);
            if (nguoidung == null)
            {
                return HttpNotFound();
            }
            return View(nguoidung);
        }

        // POST: Admin/Nguoidungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var u = Session["use"] as Ictshop.Models.Nguoidung;
            if (session.checkRoleAdmin(u))
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
            }
            Nguoidung nguoidung = db.Nguoidungs.Find(id);
            db.Nguoidungs.Remove(nguoidung);
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
