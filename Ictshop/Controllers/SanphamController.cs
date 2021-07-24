using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;

namespace Ictshop.Controllers
{
    public class SanphamController : Controller
    {
        Qlbanhang db = new Qlbanhang();

        // GET: Sanpham
        public ActionResult dtiphonepartial()
        {
            var ip = db.Sanphams.Where(n=>n.Mahang==2).ToList();
           return PartialView(ip);
        }
        public ActionResult dtsamsungpartial()
        {
            var ss = db.Sanphams.Where(n => n.Mahang == 1).ToList();
            return PartialView(ss);
        }
        public ActionResult dtxiaomipartial()
        {
            var mi = db.Sanphams.Where(n => n.Mahang == 3).ToList();
            return PartialView(mi);
        }

        public ActionResult xemchitiet(int? Masp)
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if( lstGioHang == null || lstGioHang.Count == 0)
            {
                var chitiets = db.Sanphams.SingleOrDefault(n => n.Masp == Masp);
                if (chitiets == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(chitiets);
            }
            GioHang sanpham = lstGioHang.Find(n => n.iMasp == Masp);
            var chitiet = db.Sanphams.SingleOrDefault(n => n.Masp == Masp);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.SanPhamGioHang = sanpham;
            return View(chitiet);
        }

    }

}