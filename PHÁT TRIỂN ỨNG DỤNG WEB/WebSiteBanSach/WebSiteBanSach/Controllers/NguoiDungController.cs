using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;


namespace WebSiteBanSach.Controllers
{
    public class NguoiDungController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                db.KhachHang.Add(kh);
                db.SaveChanges();
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string name = form["name"].ToString();
            string password = form["password"].ToString();
            KhachHang kh = db.KhachHang.SingleOrDefault(x => x.TaiKhoan == name && x.MatKhau == password);
            if(kh != null)
            {
                ViewBag.ThongBao = "chuc mung ban dang nhap thanh cong";
                Session["TaiKhoan"] = kh;
                return View();
            }
            ViewBag.ThongBao = "username or password incorrect";
            return View();
        }
    }
}