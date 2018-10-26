using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BAN_HANG.Models;

namespace WEBSITE_BAN_HANG.Controllers
{
    public class ThanhViensController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: ThanhViens
        public ActionResult Index()
        {
            var thanhVien = db.ThanhVien.Include(t => t.LoaiThanhVien);
            return View(thanhVien.ToList());
        }

        // GET: ThanhViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien thanhVien = db.ThanhVien.Find(id);
            if (thanhVien == null)
            {
                return HttpNotFound();
            }
            return View(thanhVien);
        }

        // GET: ThanhViens/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhVien, "MaLoaiTV", "TenLoai");
            return View();
        }

        // POST: ThanhViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThanhVien,TaiKhoan,MatKhau,HoTen,DiaChi,Email,SoDienThoai,CauHoi,CauTraLoi,MaLoaiTV")] ThanhVien thanhVien)
        {
            if (ModelState.IsValid)
            {
                db.ThanhVien.Add(thanhVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhVien, "MaLoaiTV", "TenLoai", thanhVien.MaLoaiTV);
            return View(thanhVien);
        }

        // GET: ThanhViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien thanhVien = db.ThanhVien.Find(id);
            if (thanhVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhVien, "MaLoaiTV", "TenLoai", thanhVien.MaLoaiTV);
            return View(thanhVien);
        }

        // POST: ThanhViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThanhVien,TaiKhoan,MatKhau,HoTen,DiaChi,Email,SoDienThoai,CauHoi,CauTraLoi,MaLoaiTV")] ThanhVien thanhVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thanhVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhVien, "MaLoaiTV", "TenLoai", thanhVien.MaLoaiTV);
            return View(thanhVien);
        }

        // GET: ThanhViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien thanhVien = db.ThanhVien.Find(id);
            if (thanhVien == null)
            {
                return HttpNotFound();
            }
            return View(thanhVien);
        }

        // POST: ThanhViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThanhVien thanhVien = db.ThanhVien.Find(id);
            db.ThanhVien.Remove(thanhVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
