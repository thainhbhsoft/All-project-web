using OfficeOpenXml;
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
    public class TeachersController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(db.Teacher.ToList());
            }
            return RedirectToAction("Login", "Users");
           
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherID,Name,TeacherCode,Email,UserName,PassWord")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teacher.Add(teacher);
                db.SaveChanges();
                int teachertid = db.Teacher.Max(x => x.TeacherID);
                User user = new User()
                {
                    UserName = teacher.UserName,
                    PassWord = teacher.PassWord,
                    Position = "Teacher",
                    TeacherID = teachertid
                };
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherID,Name,TeacherCode,Email,UserName,PassWord")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                User user = db.User.SingleOrDefault(x => x.TeacherID == teacher.TeacherID);
                user.UserName = teacher.UserName;
                user.PassWord = teacher.PassWord;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teacher.Find(id);
            User user = db.User.SingleOrDefault(x => x.TeacherID == teacher.TeacherID);
            db.User.Remove(user);
            db.Teacher.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ShowListSubject()
        {
            User user = Session["user"] as User;
            IEnumerable<StudentDetail> listStudent = db.StudentDetail.Where(x => x.TeacherID == user.TeacherID)
                .GroupBy(x => x.SubjectID).Select(x => x.FirstOrDefault());
                
            return View(listStudent);
        }
        public ActionResult ShowClass(int? id)
        {
            IEnumerable<StudentDetail> listStudent = db.StudentDetail.Where(x => x.SubjectID == id);
            return View(listStudent);
        }
        public ActionResult ShowResultSurvey(int? id)
        {
            List<int> lis = db.StudentDetail.Where(x => x.SubjectID == id).Select(x => x.StudentDetailID).ToList();
            ViewBag.ListPointAver = db.Survey
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            ViewBag.NameSurvey = db.ContentSurvey.Select(x => x.Text).ToList();
            ViewBag.CountSurvey = db.ContentSurvey.ToList().Count();
            ViewBag.SumStudent = db.StudentDetail.Where(x => x.SubjectID == id).ToList().Count();
            StudentDetail student_Detail = db.StudentDetail.First(x => x.SubjectID == id);
            return View(student_Detail);
        }
        [HttpPost]
        public ActionResult ImportTeacher(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            if (ImportData(out count, package))
            {
                ViewBag.message = "Import thành công";
            }
            ViewBag.countStudent = count;
            return View();
        }

        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                int startColumn = 1;
                int startRow = 2;
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                UetSurveyEntities db = new UetSurveyEntities();

                do
                {
                    data = workSheet.Cells[startRow, startColumn].Value;
                    object userName = workSheet.Cells[startRow, startColumn + 1].Value;
                    object passWord = workSheet.Cells[startRow, startColumn + 2].Value;
                    object fullName = workSheet.Cells[startRow, startColumn + 3].Value;
                    object email = workSheet.Cells[startRow, startColumn + 4].Value;

                    if (data != null)
                    {
                        var isSuccess = SaveStudent(userName.ToString(), passWord.ToString(), fullName.ToString(), email.ToString(), db);
                        if (isSuccess)
                        {
                            count++;
                            result = true;
                        }
                    }
                    startRow++;
                } while (data != null);
            }
            catch (Exception x)
            {


            }
            return result;
        }
        public bool SaveStudent(string userName, string passWord, string fullName, string email, UetSurveyEntities db)
        {
            var result = false;
            try
            {
                if (db.Teacher.Where(x => x.UserName.Equals(userName)).Count() == 0)
                {
                    var teacher = new Teacher();
                    teacher.UserName = userName;
                    teacher.PassWord = passWord;
                    teacher.Name = fullName;
                    teacher.Email = email;
                    db.Teacher.Add(teacher);
                    db.SaveChanges();
                    int teacherid = db.Teacher.Max(x => x.TeacherID);
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Teacher",
                        TeacherID = teacherid
                    };
                    db.User.Add(user);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception x)
            {

            }
            return result;

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
