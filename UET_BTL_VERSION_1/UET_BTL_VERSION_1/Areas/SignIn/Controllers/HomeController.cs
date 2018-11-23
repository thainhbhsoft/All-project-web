﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.SignIn.Controllers
{
    public class HomeController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Areas/SignIn/Views/Home/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"].ToString().Trim();
            string password = form["password"].ToString().Trim();
            User user = db.Users.FirstOrDefault(x => x.UserName == username && x.PassWord == password);
            if (user != null)
            {
                Session["user"] = user;
                if (user.Position.Equals("Teacher"))
                {
                    return RedirectToAction("Index", "Teacher", new { area = "Member" });
                }
                else if (user.Position.Equals("Student"))
                {
                    return RedirectToAction("Index", "Student",new { area = "Member" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        public PartialViewResult NamePartial()
        {
            User user = Session["user"] as User;
            if (user.Position.Equals("Teacher"))
            {
                Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                ViewBag.Name = teacher.Name.ToUpper();
                return PartialView();
            }
            else if (user.Position.Equals("Student"))
            {
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                ViewBag.Name = stu.Name.ToUpper();
                return PartialView();
            }
            else
            {
                return PartialView();
            }
        }

        public PartialViewResult MenuPartial()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                ViewBag.position = user.Position.ToString();
            }
            return PartialView();
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