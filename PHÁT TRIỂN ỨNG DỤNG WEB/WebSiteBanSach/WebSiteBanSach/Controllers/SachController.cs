using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    public class SachController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public PartialViewResult SachMoiPartial()
        {
            var listSachMoi = db.Sach.Take(3).ToList();
            return PartialView(listSachMoi);
        }
        public ViewResult XemChiTiet(int MaSach = 1)
        {
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == MaSach);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        
    }
}