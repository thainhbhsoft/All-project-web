using OfficeOpenXml;
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
    public class StudentsController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        // GET: Students
        public ActionResult Index(int? page)
        {
            if (Session["user"] != null)
            {
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                ViewBag.sum = db.Student.Count();
                return View(db.Student.ToList().ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Users");
           
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Student> listKQ = db.Student.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            // phân trang
            int pageSize = 200;
            int pageNumber = (page ?? 1);
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sinh viên nào";
                return View(db.Student.ToList().ToPagedList(pageNumber, pageSize));
            }
            ViewBag.messageSearch = "Kết quả tìm kiếm với từ khóa : " + sTuKhoa;
            ViewBag.sum = listKQ.Count();
            return View(listKQ.ToPagedList(pageNumber, pageSize));
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
        public ActionResult DeleteConfirmed(FormCollection f)
        {
            int id = int.Parse(f["STUID"].ToString());
            Student student = db.Student.SingleOrDefault(x => x.StudentID == id);
            User user = db.User.SingleOrDefault(x => x.StudentID == id);
            IEnumerable<StudentDetail> list2 = db.StudentDetail.Where(x => x.StudentID == id);
            foreach (var item in list2)
            {
                db.StudentDetail.Remove(item);
                IEnumerable<Survey> list3 = db.Survey.Where(x => x.StudentDetailID == item.StudentDetailID);
                try
                {
                    foreach (var item3 in list3)
                    {
                        db.Survey.Remove(item3);
                    }
                }
                catch (Exception)
                {

                }

            }
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
        [HttpPost]
        public ActionResult ImportStudent(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            if(ImportData(out count, package))
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
                    object course = workSheet.Cells[startRow, startColumn + 5].Value;

                    if (data != null)
                    {
                        var isSuccess = SaveStudent(userName.ToString(), passWord.ToString(), fullName.ToString(), email.ToString(), course.ToString(), db);
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
        public bool SaveStudent(string userName, string passWord, string fullName, string email, string course, UetSurveyEntities db)
        {
            var result = false;
            try
            {
                if (db.Student.Where(x => x.UserName.Equals(userName)).Count() == 0)
                {
                    var stu = new Student();
                    stu.UserName = userName;
                    stu.PassWord = passWord;
                    stu.StudentCode = userName;
                    stu.Name = fullName;
                    stu.Email = email;
                    stu.Course = course;
                    db.Student.Add(stu);
                    db.SaveChanges();
                    int studentid = db.Student.Max(x => x.StudentID);
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Student",
                        StudentID = studentid
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
