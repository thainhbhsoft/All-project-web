﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                ViewBag.SumStudents = db.Students.ToList().Count();
                ViewBag.SumSubjects = db.Subjects.ToList().Count();
                ViewBag.SumSurveys = db.Surveys.GroupBy(x => x.StudentDetailID).Count();
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                return View();
            }
            return RedirectToAction("Login", "Home",new { area = "SignIn"});
        }
    }
}