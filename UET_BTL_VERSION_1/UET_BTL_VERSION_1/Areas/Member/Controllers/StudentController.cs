using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Member.Controllers
{
    [AuthorizeBusiness]
    public class StudentController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        public ActionResult Index()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                Student stu = db.Students.FirstOrDefault(s => s.StudentID == user.StudentID);
                ViewBag.SumSubject = stu.StudentDetail.Count();
                List<int> listID = stu.StudentDetail.Select(y => y.StudentDetailID).ToList();
                ViewBag.SumSubjectSurvey = db.Surveys.Where(x => listID.Any(y => y == x.StudentDetailID)).GroupBy(x => x.StudentDetailID).Count();
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
                return View();
            }
            return RedirectToAction("Login","Home",new { area = "SignIn"});
        }

        public ActionResult ShowInforStudent()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                Student student = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                return View(student);
            }
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        [HttpPost]
        public ActionResult ShowInforStudent(FormCollection f)
        {
            User user = Session["user"] as User;
            string pass = f["passOld"].ToString();
            if (user != null)
            {
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                if ((pass != user.PassWord) || (f["passNew1"].ToString().Trim() != f["passNew2"].ToString().Trim()))
                {
                    ViewBag.Message = "Mật khẩu cũ không đúng hoặc hai mật khẩu mới không khớp nhau!";
                    return View(stu);
                }
                User u = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
                u.PassWord = f["passNew1"].ToString();
                stu.PassWord = f["passNew1"].ToString();
                db.SaveChanges();
                ViewBag.Message = "Đã đổi mật khẩu thành công";
                return View(stu);
            }
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        public ActionResult ShowListSubject()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                IEnumerable<StudentDetail> listStudent = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID).StudentDetail.ToList();
                return View(listStudent);
            }
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        public ActionResult SurveySubject(int? id, int? stID)
        {
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id && x.StudentID == stID);
            if (db.Surveys.Any(x => x.StudentDetailID == student_Detail.StudentDetailID))
            {
                ViewBag.Message = "Bạn đã đánh giá môn học này";
                return View(student_Detail);
            }
            ViewBag.Count = db.ContentSurveys.ToList().Count();
            ViewBag.ContentSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            return View(student_Detail);
        }

        [HttpPost]
        public ActionResult SurveySubject(FormCollection form)
        {
            ViewBag.Count = db.ContentSurveys.ToList().Count();
            ViewBag.ContentSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            int k = int.Parse(form["id"]);
            int stDetailID = int.Parse(form["studentDetailID"]);
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == k);
            if (form.Count < db.ContentSurveys.ToList().Count() + 3)
            {
                ViewBag.Message2 = "Bạn cần đánh giá đủ các tiêu chí";
                return View(student_Detail);
            }
            int i = 0;
            foreach (var item in db.ContentSurveys)
            {
                Survey survey = new Survey();
                survey.StudentDetailID = stDetailID;
                survey.ContentSurveyID = item.ContentSurveyID;
                survey.Point = int.Parse(form[i++]);
                db.Surveys.Add(survey);
            }
            StudentDetail stDetail = db.StudentDetails.FirstOrDefault(x => x.StudentDetailID == stDetailID);
            stDetail.NoteSurvey = form["note"].ToString();
            db.SaveChanges();
            return RedirectToAction("ShowListSubject");
        }
    }
}