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
    public class HomeController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: Home
        public ActionResult Index(int? page)
        {
            // Tạo biến số sản phẩm trên trang
            int pageSize = 3;
            // tạo biến số trang
            int pageNumber = (page ?? 1);
            return View(db.Sach.Where(x => x.Moi == 1).OrderBy(x => x.GiaBan).ToPagedList(pageNumber, pageSize));
        }
       
    }
}