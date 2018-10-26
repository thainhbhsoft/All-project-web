using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBSITE_BAN_HANG.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["PageView"].ToString();
            ViewBag.SoNguoiOnline = HttpContext.Application["Online"].ToString();
            return View();
        }
    }
}
