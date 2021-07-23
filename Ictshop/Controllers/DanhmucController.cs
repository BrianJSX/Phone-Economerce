using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;
using PagedList;

namespace Ictshop.Controllers
{
    public class DanhmucController : Controller
    {
        Qlbanhang db = new Qlbanhang();
        // GET: Danhmuc
        public ActionResult DanhmucPartial()
        {
            var category = db.Hangsanxuats.ToList();
            return PartialView(category);
        }
        public ActionResult List(int? id, int? page)
        {
            if (page == null) page = 1;
            var danhmuc = db.Sanphams.Where(n => n.Mahang == id).OrderByDescending(n => n.Masp);
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(danhmuc.ToPagedList(pageNumber, pageSize));
        }
    }
}