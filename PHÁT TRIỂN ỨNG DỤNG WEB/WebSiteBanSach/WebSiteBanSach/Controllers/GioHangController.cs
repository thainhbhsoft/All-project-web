using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    // Class giỏ hàng controller
    public class GioHangController : Controller
    {
        //khởi tạo một database quản lý bán sách chứa các bảng
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        #region giỏ hàng
        // lấy ra danh sách các giỏ hàng
        public List<GioHang> LayGioHang()
        {
            // lấy ra từ session để gán vào danh sách giỏ hàng
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang == null)
            {
                // session ko tồn tại nên khỏi tạo danh sách mới và gán vào session
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            // trả về một danh sách trống hoặc danh sách giỏ hàng ( không phải danh sách null)
            return listGioHang;
        }
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            // kiểm tra xem người dùng có get tùy tiện từ url không
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == iMaSach);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lấy ra danh sách giỏ hàng trống hoặc đã có 
            List<GioHang> listGioHang = LayGioHang();
            GioHang giohang = listGioHang.Find(x => x.iMaSach == iMaSach);
            if(giohang == null)
            {
                giohang = new GioHang(iMaSach);
                listGioHang.Add(giohang);
                return Redirect(strURL);
            }
            else
            {
                giohang.iSoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == iMaSP);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listGioHang = LayGioHang();
            GioHang giohang = listGioHang.SingleOrDefault(x => x.iMaSach == iMaSP);
            if(giohang != null)
            {
                giohang.iSoLuong = int.Parse( f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == iMaSP);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listGioHang = LayGioHang();
            GioHang giohang = listGioHang.SingleOrDefault(x => x.iMaSach == iMaSP);
            if (giohang != null)
            {
                listGioHang.RemoveAll(x => x.iMaSach == iMaSP);
            }
            if(listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> listGioHang = LayGioHang();
            return View(listGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(x => x.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                dTongTien = listGioHang.Sum(x => x.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHangPartial()
        {
           if(TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> listGioHang = LayGioHang();
            return View(listGioHang);
        }
        #endregion
        #region đặt hàng
        [HttpPost]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập
            if(Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            //Kiểm tra giỏ hàng
            if(Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
            DonHang donhang = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> giohang = LayGioHang();
            donhang.MaKH = kh.MaKH;
            donhang.NgayDat = DateTime.Now;
            db.DonHang.Add(donhang);
            db.SaveChanges();
            //Them chi tiet don hang
            foreach(var item in giohang)
            {
                ChiTietDonHang ctDonHang = new ChiTietDonHang();
                ctDonHang.MaDonHang = donhang.MaDonHang;
                ctDonHang.MaSach = item.iMaSach;
                ctDonHang.SoLuong = item.iSoLuong;
                ctDonHang.DonGia = (decimal)item.dDonGia;
                db.ChiTietDonHang.Add(ctDonHang);
            }
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        #endregion
    }
}