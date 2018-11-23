using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Member.Controllers
{
    public class TeacherController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        public ActionResult Index()
        {
                User user = Session["user"] as User;
                if (user != null)
                {
                    ViewBag.SumStudents = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.Count();
                    ViewBag.SumSubjects = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.GroupBy(x => x.SubjectID).Count();
                    ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                    return View();
                }
                return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        public ActionResult ShowInforTeacher()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                Teacher teacher = db.Teachers.SingleOrDefault(x => x.TeacherID == user.TeacherID);
                return View(teacher);
            }
            return RedirectToAction("Login","Home",new { area = "SignIn" });
        }

        [HttpPost]
        public ActionResult ShowInforTeacher(FormCollection f)
        {
            User user = Session["user"] as User;
            string pass = f["passOld"].ToString();
            if (user != null)
            {
                Teacher teacher = db.Teachers.SingleOrDefault(x => x.TeacherID == user.TeacherID);
                if ((pass != user.PassWord) || (f["passNew1"].ToString().Trim() != f["passNew2"].ToString().Trim()))
                {
                    ViewBag.Message = "Mật khẩu cũ không đúng hoặc hai mật khẩu mới không khớp nhau!";
                    return View(teacher);
                }
                User u = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                u.PassWord = f["passNew1"].ToString();
                teacher.PassWord = f["passNew1"].ToString();
                db.SaveChanges();
                ViewBag.Message = "Đã đổi mật khẩu thành công";
                return View(teacher);
            }
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        public ActionResult ShowListSubject()
        {
            User user = Session["user"] as User;
            IEnumerable<StudentDetail> listStudent = db.StudentDetails.Where(x => x.TeacherID == user.TeacherID)
                .GroupBy(x => x.SubjectID).Select(x => x.FirstOrDefault());

            return View(listStudent);
        }

        public ActionResult ShowClass(int? id)
        {
            IEnumerable<StudentDetail> listStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList();
            return View(listStudent);
        }

        public ActionResult ShowResultSurvey(int? id)
        {
            List<int> lis = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.Select(x => x.StudentDetailID).ToList();
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id);
            ViewBag.hasSurvey = db.Surveys.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            ViewBag.SumStudent = db.Subjects.FirstOrDefault(x => x.SubjectID == id).StudentDetail.ToList().Count();
            if (ViewBag.hasSurvey == 0)
            {
                return View(student_Detail);
            }
            ViewBag.ListPointAver = db.Surveys
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            ViewBag.NameSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            ViewBag.CountSurvey = db.ContentSurveys.ToList().Count();

            return View(student_Detail);
        }

    }
}