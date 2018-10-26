using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
using PagedList;
using PagedList.Mvc;
namespace WebSiteBanSach.Controllers
{
    public class TimKiemController : Controller
    {
        //khởi tạo một database quản lý bán sách chứa các bảng
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KetQuaTimKiem(int? page, string tukhoa)
        {
           
            List<Sach> listKQ = db.Sach.Where(x => x.TenSach.Contains(tukhoa)).ToList();
            // phân trang
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Sach.OrderBy(x => x.TenSach).ToPagedList(pageNumber, pageSize));

            }
            return View(listKQ.OrderBy(x => x.TenSach).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Sach> listKQ = db.Sach.Where(x => x.TenSach.Contains(sTuKhoa)).ToList();
            // phân trang
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            if(listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Sach.OrderBy(x => x.TenSach).ToPagedList(pageNumber, pageSize));

            }
            return View(listKQ.OrderBy(x => x.TenSach).ToPagedList(pageNumber, pageSize));
        }
    }
}