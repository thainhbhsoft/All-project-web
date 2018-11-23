using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class TeacherManagerController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();

        public ActionResult Index(int? page)
        {
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                ViewBag.sum = db.Teachers.Count();
                return View(db.Teachers.ToList().ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.tukhoa = sTuKhoa;
            List<Teacher> listKQ = db.Teachers.Where(x => x.Name.Contains(sTuKhoa)).ToList();
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
                Teacher tea = new Teacher();
                tea.UserName = form["UserName"].ToString();
                tea.Name = form["Name"].ToString();
                tea.TeacherCode = "123456";
                tea.Email = form["Email"].ToString();
                tea.PassWord = form["PassWord"].ToString();
                db.Teachers.Add(tea);
                db.SaveChanges();
                int teacherid = db.Teachers.Max(x => x.TeacherID);
                User user = new User()
                {
                    UserName = tea.UserName,
                    PassWord = tea.PassWord,
                    Position = "Teacher",
                    TeacherID = teacherid
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
            int idTeacher = int.Parse(form["idTeacher"].ToString());
            string username = form["UserName"].ToString();
            if (db.Users.Any(x => x.UserName.Equals(username) && x.TeacherID != idTeacher))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User user = db.Users.FirstOrDefault(x => x.TeacherID == idTeacher);
                user.UserName = username;
                user.PassWord = form["PassWord"].ToString();
                Teacher tea = db.Teachers.FirstOrDefault(x => x.TeacherID == idTeacher);
                tea.UserName = username;
                tea.Name = form["Name"].ToString();
                tea.TeacherCode = "123456";
                tea.Email = form["Email"].ToString();
                tea.PassWord = form["PassWord"].ToString();
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
            Teacher teacher = db.Teachers.Find(id);
            bool a = db.StudentDetails.Any(x => x.TeacherID == id);
            if (a)
            {
                return Json(new { status = 0, teacher = teacher }, JsonRequestBehavior.AllowGet);
            }
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return Json(new { status = 1, teacher = teacher }, JsonRequestBehavior.AllowGet);
        }

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
                    object checkColumn = workSheet.Cells[startRow, startColumn + 5].Value;
                    if (checkColumn != null)
                    {
                        throw new Exception("error");
                    }
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
    }
}