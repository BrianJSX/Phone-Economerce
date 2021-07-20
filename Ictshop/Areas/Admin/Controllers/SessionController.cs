using Ictshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ictshop.Areas.Admin.Controllers
{
    public class SessionController : Controller
    {
        public bool checkRoleAdmin(Nguoidung u)
        {
            if (u == null || u.IDQuyen != 2)
            {
                return true;
            }
            return false;
        }
    }
}