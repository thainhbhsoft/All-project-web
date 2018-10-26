using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    public class ChuDeController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: ChuDe
        public PartialViewResult ChuDePartial()
        {
            var listSachMoi = db.ChuDe.Take(3).ToList();
            return PartialView(listSachMoi);
        }
        public ViewResult SachTheoChuDe(int MaChuDe = 0)
        {
            ChuDe chude = db.ChuDe.SingleOrDefault(x => x.MaChuDe == MaChuDe);
            if(chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Sach> listSach = db.Sach.Where(x => x.MaChuDe == MaChuDe).OrderBy(x => x.GiaBan).ToList();
            return View(listSach);
        }
        public ViewResult DanhMucChuDe()
        {
           
            return View(db.ChuDe.ToList());
        }
    }
}