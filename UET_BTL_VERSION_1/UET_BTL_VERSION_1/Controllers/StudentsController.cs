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
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Controllers
{
    public class StudentsController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();
        // GET: Students
        public ActionResult Index(int? page)
        {
            if (Session["user"] != null)
            {
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                ViewBag.sum = db.Students.Count();
                return View(db.Students.ToList().ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Users");
           
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Student> listKQ = db.Students.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            // phân trang
            int pageSize = 200;
            int pageNumber = (page ?? 1);
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sinh viên nào";
                return View(db.Students.ToList().ToPagedList(pageNumber, pageSize));
            }
            ViewBag.messageSearch = "Kết quả tìm kiếm với từ khóa : " + sTuKhoa;
            ViewBag.sum = listKQ.Count();
            return View(listKQ.ToPagedList(pageNumber, pageSize));
        }
       
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string username = form["UserName"].ToString();
            if (db.Users.Any(x => x.UserName.Equals(username)))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Student stu = new Student();
                stu.Name = form["Name"].ToString();
                stu.Course = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName = form["UserName"].ToString();
                stu.Email = form["Email"].ToString();
                stu.PassWord = form["PassWord"].ToString();
                db.Students.Add(stu);
                db.SaveChanges();
                int studentid = db.Students.Max(x => x.StudentID);
                User user = new User()
                {
                    UserName = stu.UserName,
                    PassWord = stu.PassWord,
                    Position = "Student",
                    StudentID = studentid
                };
                db.Users.Add(user);
                db.SaveChanges();
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string username = form["UserName"].ToString();
            int idStudent = int.Parse(form["idStudent"].ToString());
            if (db.Users.Where(x => x.UserName.Equals(username) && x.StudentID != idStudent).ToList().Count() > 0)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User user = db.Users.FirstOrDefault(x => x.StudentID == idStudent);
                user.UserName = username;
                user.PassWord = form["PassWord"].ToString();
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == idStudent);
                stu.Name = form["Name"].ToString();
                stu.Course = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName = username;
                stu.Email = form["Email"].ToString();
                stu.PassWord = form["PassWord"].ToString();
                db.SaveChanges();
                return Json(new { status = 2 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return Json(student, JsonRequestBehavior.AllowGet);
        }
        // POST: Students/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.FirstOrDefault(x => x.StudentID == id);
            User user = db.Users.FirstOrDefault(x => x.StudentID == id);
            IEnumerable<StudentDetail> list2 = db.StudentDetails.Where(x => x.StudentID == id);
            foreach (var item in list2)
            {
                IEnumerable<Survey> list3 = db.Surveys.Where(x => x.StudentDetailID == item.StudentDetailID);
                try
                {
                    db.Surveys.RemoveRange(list3);
                }
                catch (Exception)
                {

                }
            }
            db.StudentDetails.RemoveRange(list2);
            db.Users.Remove(user);
            db.Students.Remove(student);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowCountResult()
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
            return RedirectToAction("Login", "Users");
        }
        public ActionResult ShowListSubject()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                IEnumerable<StudentDetail> listStudent = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID).StudentDetail.ToList();
                return View(listStudent);
            }
            return RedirectToAction("Login", "Users");
        }
        public ActionResult ShowInforStudent()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                Student student = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                return View(student);
            }
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public ActionResult ShowInforStudent(FormCollection f)
        {
            User user = Session["user"] as User;
            string pass = f["passOld"].ToString();
            if (user != null)
            {
                Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                if ((pass != user.PassWord)  || (f["passNew1"].ToString().Trim() != f["passNew2"].ToString().Trim()))
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
            return RedirectToAction("Login", "Users");
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
            if(form.Count < db.ContentSurveys.ToList().Count() + 3)
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
            StudentDetail stDetail =  db.StudentDetails.FirstOrDefault(x => x.StudentDetailID == stDetailID);
            stDetail.NoteSurvey = form["note"].ToString();
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
                UetSurveyDbContext db = new UetSurveyDbContext();

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
        public bool SaveStudent(string userName, string passWord, string fullName, string email, string course, UetSurveyDbContext db)
        {
            var result = false;
            try
            {
                if (db.Students.Where(x => x.UserName.Equals(userName)).Count() == 0)
                {
                    var stu = new Student();
                    stu.UserName = userName;
                    stu.PassWord = passWord;
                    stu.StudentCode = userName;
                    stu.Name = fullName;
                    stu.Email = email;
                    stu.Course = course;
                    db.Students.Add(stu);
                    db.SaveChanges();
                    int studentid = db.Students.Max(x => x.StudentID);
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Student",
                        StudentID = studentid
                    };
                    db.Users.Add(user);
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
