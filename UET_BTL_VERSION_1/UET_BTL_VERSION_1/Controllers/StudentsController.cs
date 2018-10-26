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
    public class StudentsController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        // GET: Students
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(db.Student.ToList());
            }
            return RedirectToAction("Login", "Users");
           
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,Name,StudentCode,Course,DateOfBirth,Email,UserName,PassWord")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Student.Add(student);
                db.SaveChanges();
                int studentid = db.Student.Max(x => x.StudentID);
                User user = new User()
                {
                    UserName = student.UserName,
                    PassWord = student.PassWord,
                    Position = "Student",
                    StudentID = studentid
                };
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,Name,StudentCode,Course,DateOfBirth,Email,UserName,PassWord")] Student student)
        {
            if (ModelState.IsValid)
            {
                User user = db.User.SingleOrDefault(x => x.StudentID == student.StudentID);
                    user.UserName = student.UserName;
                    user.PassWord = student.PassWord;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Student.Find(id);
            User user = db.User.SingleOrDefault(x => x.StudentID == student.StudentID);
            db.User.Remove(user);
            db.Student.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ShowListSubject()
        {
            User user = Session["user"] as User;
            IEnumerable<StudentDetail> listStudent = db.StudentDetail.Where(x => x.StudentID == user.StudentID);

            return View(listStudent);
        }
        public ActionResult SurveySubject(int? id, int? stID)
        {
            StudentDetail student_Detail = db.StudentDetail.First(x => x.SubjectID == id && x.StudentID == stID);
            if (db.Survey.Any(x => x.StudentDetailID == student_Detail.StudentDetailID))
            {
                ViewBag.check = 1;
                ViewBag.Message = "Bạn đã đánh giá môn học này";
                return View(student_Detail);
            }
            ViewBag.Count = db.ContentSurvey.ToList().Count();
            ViewBag.ContentSurvey = db.ContentSurvey.Select(x => x.Text).ToList();
            return View(student_Detail);
        }
        [HttpPost]
        public ActionResult SurveySubject(FormCollection form)
        {
            ViewBag.Count = db.ContentSurvey.ToList().Count();
            ViewBag.ContentSurvey = db.ContentSurvey.Select(x => x.Text).ToList();
            int k = int.Parse(form["id"]);
            int stDetailID = int.Parse(form["studentDetailID"]);
            StudentDetail student_Detail = db.StudentDetail.First(x => x.SubjectID == k);
            if(form.Count < db.ContentSurvey.ToList().Count() + 2)
            {
                ViewBag.Message = "Bạn cần đánh giá đủ các tiêu chí";
                return View(student_Detail);
            }
            for (int i = 0; i < ViewBag.Count; i++)
            {
                Survey survey = new Survey();
                survey.StudentDetailID = stDetailID;
                survey.ContentSurveyID = i + 1;
                survey.Point = int.Parse(form[i]);
                db.Survey.Add(survey);
            }
            db.SaveChanges();
            return RedirectToAction("ShowListSubject");
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
