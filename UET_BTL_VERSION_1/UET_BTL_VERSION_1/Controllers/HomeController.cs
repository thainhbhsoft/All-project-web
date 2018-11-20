using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Controllers
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
            return RedirectToAction("Login", "Users");
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
    }
}
