using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UET_BTL_VERSION_1.Models;

namespace UET_BTL_VERSION_1.Controllers
{
    public class UsersController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"].ToString().Trim();
            string password = form["password"].ToString().Trim();
            User user = db.User.SingleOrDefault(x => x.UserName == username && x.PassWord == password);
            if (user != null)
            {
                Session["user"] = user;
                if(user.Position == "Teacher")
                {
                    return RedirectToAction("ShowListSubject", "Teachers");
                }else if(user.Position == "Student")
                {
                    return RedirectToAction("ShowListSubject", "Students");
                }
                else
                {
                    return RedirectToAction("Index", "Students");
                }
               
            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Login","Users");
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
