using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using UET_BTL_VERSION_1.Models;

namespace UET_BTL_VERSION_1.Controllers
{
    public class HomeController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();
       
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                ViewBag.SumStudents = db.Student.ToList().Count();
                ViewBag.SumSubjects = db.Subject.ToList().Count();
                ViewBag.SumSurveys = db.Survey.GroupBy(x => x.StudentDetailID).Count();
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                return View();
            }
            return RedirectToAction("Login", "Users");
        }
        public PartialViewResult NamePartial()
        {
            User user = Session["user"] as User;
            if(user.Position == "Teacher")
            {
                Teacher teacher = db.Teacher.SingleOrDefault(x => x.TeacherID == user.TeacherID);
                ViewBag.Name = teacher.Name;
                return PartialView();
            }
            else if (user.Position == "Student")
            {
                Student stu = db.Student.SingleOrDefault(x => x.StudentID == user.StudentID);
                ViewBag.Name = stu.Name;
                return PartialView();
            }
            else
            {
                return  PartialView();
            }
        }
    }
}
