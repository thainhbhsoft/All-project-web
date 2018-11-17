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
    public class TeachersController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();
        // GET: Teachers
        public ActionResult Index(int? page)
        {
            if (Session["user"] != null)
            {
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                ViewBag.sum = db.Teachers.Count();
                return View(db.Teachers.ToList().ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Users");
           
        }
        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Teacher> listKQ = db.Teachers.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            // phân trang
            int pageSize = 200;
            int pageNumber = (page ?? 1);
            if (listKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy giang viên nào";
                return View(db.Teachers.ToList().ToPagedList(pageNumber, pageSize));
            }
            ViewBag.messageSearch = "Kết quả tìm kiếm với từ khóa : " + sTuKhoa; 
            ViewBag.sum = listKQ.Count();
            return View(listKQ.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShowCountResult()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                ViewBag.SumStudents = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.Count();
                ViewBag.SumSubjects = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID).StudentDetail.GroupBy(x => x.SubjectID).Count();
                ViewBag.SumUserOnline = HttpContext.Application["Online"].ToString();
            }
            return View();
        }
        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherID,Name,TeacherCode,Email,UserName,PassWord")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                if (db.Teachers.Any(x => x.UserName == teacher.UserName))
                {
                    ViewBag.error = "Tên đăng nhập đã tồn tại";
                    return View(teacher);
                }
                else
                {
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    int teachertid = db.Teachers.Max(x => x.TeacherID);
                    User user = new User()
                    {
                        UserName = teacher.UserName,
                        PassWord = teacher.PassWord,
                        Position = "Teacher",
                        TeacherID = teachertid
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }
        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherID,Name,TeacherCode,Email,UserName,PassWord")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(x => x.TeacherID == teacher.TeacherID);
                user.UserName = teacher.UserName;
                user.PassWord = teacher.PassWord;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }
        // GET: Teachers/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            bool a = db.StudentDetails.Any(x => x.TeacherID == id);
            if (a)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return Json(new { status = 1,teacher = teacher }, JsonRequestBehavior.AllowGet);
        }
        // POST: Teachers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            User user = db.Users.FirstOrDefault(x => x.TeacherID == teacher.TeacherID);
            db.Users.Remove(user);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ShowInforTeacher()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                Teacher teacher = db.Teachers.SingleOrDefault(x => x.TeacherID == user.TeacherID);
                return View(teacher);
            }
            return RedirectToAction("Login", "Users");
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
            return RedirectToAction("Login", "Users");
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
                UetSurveyDbContext db = new UetSurveyDbContext();

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
        public bool SaveStudent(string userName, string passWord, string fullName, string email, UetSurveyDbContext db)
        {
            var result = false;
            try
            {
                if (db.Teachers.Where(x => x.UserName.Equals(userName)).Count() == 0)
                {
                    var teacher = new Teacher();
                    teacher.UserName = userName;
                    teacher.PassWord = passWord;
                    teacher.Name = fullName;
                    teacher.Email = email;
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    int teacherid = db.Teachers.Max(x => x.TeacherID);
                    User user = new User()
                    {
                        UserName = userName,
                        PassWord = passWord,
                        Position = "Teacher",
                        TeacherID = teacherid
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
