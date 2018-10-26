using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebSiteBanSach.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(db.Sach.ToList().OrderBy(x => x.MaSach).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //  ĐƯA dữ liệu vào dropdown list
            ViewBag.MaChuDe = new SelectList(db.ChuDe.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBan.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Sach sach, HttpPostedFileBase fileUpload)
        {
          
            //  ĐƯA dữ liệu vào dropdown list
            ViewBag.MaChuDe = new SelectList(db.ChuDe.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBan.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chọn Hình ảnh";
                return View();
            }
            if (ModelState.IsValid)
            {
                // Lưu tên file
                var fileName = Path.GetFileName(fileUpload.FileName);
                // lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/HinhAnhSP"), fileName);
                //Kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                    sach.AnhBia = fileUpload.FileName;
                    db.Sach.Add(sach);
                    db.SaveChanges();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChinhSua(int MaSach)
        {
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //  ĐƯA dữ liệu vào dropdown list
            ViewBag.MaChuDe = new SelectList(db.ChuDe.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBan.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Sach sach)
        {
            //  ĐƯA dữ liệu vào dropdown list
            ViewBag.MaChuDe = new SelectList(db.ChuDe.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", sach.MaChuDe);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBan.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            // thêm vào csdl
            if (ModelState.IsValid)
            {
                // thực hiện cập nhật trong model
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int MaSach)
        {
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        public ActionResult Xoa(int MaSach)
        {
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost,ActionName("Xoa")]
        public ActionResult XacNhanXoa(FormCollection f)
        {
            int maSach = int.Parse(f["masach"]);
            Sach sach = db.Sach.SingleOrDefault(x => x.MaSach == maSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Sach.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}