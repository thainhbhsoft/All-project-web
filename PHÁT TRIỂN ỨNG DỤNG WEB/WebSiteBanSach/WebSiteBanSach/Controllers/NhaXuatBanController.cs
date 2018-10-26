using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    public class NhaXuatBanController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: ChuDe
        public PartialViewResult NhaXuatBanPartial()
        {
            var listSachMoi = db.NhaXuatBan.Take(3).ToList();
            return PartialView(listSachMoi);
        }
        public ViewResult SachTheoNXB(int MaNXB)
        {
            NhaXuatBan nhaxuatban = db.NhaXuatBan.SingleOrDefault(x => x.MaNXB == MaNXB);
            if (nhaxuatban == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Sach> listSach = db.Sach.Where(x => x.MaNXB == MaNXB).OrderBy(x => x.GiaBan).ToList();
            return View(listSach);
        }
        public ViewResult DanhMucNXB()
        {

            return View(db.NhaXuatBan.ToList());
        }
    }
}