using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using PagedList;
using UET_BTL.Model.Entities;
using System.Net;
using OfficeOpenXml;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    public class StudentManagerController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        [HttpGet]
        public ActionResult Index(int? page)
        {
                int pageSize   = 20;
                int pageNumber = (page ?? 1);
                ViewBag.sum    = db.Students.Count();
                return View(db.Students.ToList().ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Student> listKQ = db.Students.Where(x => x.Name.Contains(sTuKhoa)).ToList();
            int pageSize   = 200;
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
                Student stu     = new Student();
                stu.Name        = form["Name"].ToString();
                stu.Course      = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName    = form["UserName"].ToString();
                stu.Email       = form["Email"].ToString();
                stu.PassWord    = form["PassWord"].ToString();
                db.Students.Add(stu);
                db.SaveChanges();
                int studentid   = db.Students.Max(x => x.StudentID);
                User user = new User()
                {
                    UserName    = stu.UserName,
                    PassWord    = stu.PassWord,
                    Position    = "Student",
                    StudentID   = studentid
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
            if (db.Users.Any(x => x.UserName.Equals(username) && x.StudentID != idStudent))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User user       = db.Users.FirstOrDefault(x => x.StudentID == idStudent);
                user.UserName   = username;
                user.PassWord   = form["PassWord"].ToString();
                Student stu     = db.Students.FirstOrDefault(x => x.StudentID == idStudent);
                stu.Name        = form["Name"].ToString();
                stu.Course      = form["Course"].ToString();
                stu.StudentCode = form["StudentCode"].ToString();
                stu.UserName    = username;
                stu.Email       = form["Email"].ToString();
                stu.PassWord    = form["PassWord"].ToString();
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.FirstOrDefault(x => x.StudentID == id);
            User user       = db.Users.FirstOrDefault(x => x.StudentID == id);
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

        [HttpPost]
        public ActionResult ImportStudent(HttpPostedFileBase fileUpload)
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
                    var stu         = new Student();
                    stu.UserName    = userName;
                    stu.PassWord    = passWord;
                    stu.StudentCode = userName;
                    stu.Name        = fullName;
                    stu.Email       = email;
                    stu.Course      = course;
                    db.Students.Add(stu);
                    db.SaveChanges();
                    int studentid   = db.Students.Max(x => x.StudentID);
                    User user = new User()
                    {
                        UserName    = userName,
                        PassWord    = passWord,
                        Position    = "Student",
                        StudentID   = studentid
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
    }
}