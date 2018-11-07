using OfficeOpenXml;
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
    public class SubjectsController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();
        // GET: Subjects
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(db.Subjects.ToList());
            }
            return RedirectToAction("Login", "Users");
        }
        // GET: Subjects/Details/5
        public ActionResult ShowClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<StudentDetail> listStudent = db.StudentDetails.Where(x => x.SubjectID == id);
            return View(listStudent);
           
        }
        public ActionResult ResultSurveyEveryStudent(int? id)
        {
            ViewBag.hasSurvey = db.Surveys.Where(x => x.StudentDetailID == id).ToList().Count();
            if (ViewBag.hasSurvey == 0)
            {
                return View();
            }
            ViewBag.ListPoint = db.Surveys
              .Where(x => x.StudentDetailID == id)
              .Select(x => x.Point).ToList();
            ViewBag.NameSurvey = db.ContentSurveys.Select(x => x.Text).ToList();
            ViewBag.CountSurvey = db.ContentSurveys.ToList().Count();
            ViewBag.note = db.StudentDetails.SingleOrDefault(x => x.StudentDetailID == id).NoteSurvey;
            return View();
        }
        public ActionResult ShowResultSurvey(int? id)
        {
            List<int> lis = db.StudentDetails.Where(x => x.SubjectID == id).Select(x => x.StudentDetailID).ToList();
            ViewBag.hasSurvey = db.Surveys.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            StudentDetail student_Detail = db.StudentDetails.First(x => x.SubjectID == id);
            ViewBag.SumStudent = db.StudentDetails.Where(x => x.SubjectID == id).ToList().Count();
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
        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }
        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            IEnumerable<StudentDetail> list = db.StudentDetails.Where(x => x.SubjectID == id);
            foreach (var item in list)
            {
                db.StudentDetails.Remove(item);
                IEnumerable<Survey> list2 = db.Surveys.Where(x => x.StudentDetailID == item.StudentDetailID);
                try
                {
                    foreach (var item2 in list2)
                    {
                        db.Surveys.Remove(item2);
                    }
                }
                catch (Exception)
                {

                }
               
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ImportSubject(HttpPostedFileBase fileUpload)
        {
            ViewBag.message = "Import không thành công";
            int count = 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            if (ImportData(out count, package))
            {
                ViewBag.message = "Import thành công";
            }
            return View();
        }
        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                int startColumn = 1;
                int startRow = 12;
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                UetSurveyDbContext db = new UetSurveyDbContext();
                object teacherName = workSheet.Cells[7,3].Value;
                object subjectName = workSheet.Cells[10,3].Value;
                object subjectCode = workSheet.Cells[9,3].Value;
                object classRoom = workSheet.Cells[8,6].Value;
                object creditNumber = workSheet.Cells[9,6].Value;
                object time = workSheet.Cells[8,3].Value;
                 if (db.Subjects.Where(x => x.SubjectCode.ToLower().Equals(subjectCode.ToString().ToLower())).Count() == 0)
                {
                    Subject sub = new Subject();
                    sub.Name = subjectName.ToString();
                    sub.SubjectCode = subjectCode.ToString();
                    sub.ClassRoom = classRoom.ToString();
                    sub.CreditNumber = int.Parse(creditNumber.ToString());
                    sub.TimeTeach = time.ToString();
                    db.Subjects.Add(sub);
                    db.SaveChanges();
                    int subID = db.Subjects.Max(x => x.SubjectID);
                    int teacherID = db.Teachers.SingleOrDefault(x => x.Name.ToLower().Equals(teacherName.ToString().ToLower())).TeacherID;

                    do
                    {
                        data = workSheet.Cells[startRow, startColumn].Value;
                        string userName = workSheet.Cells[startRow, startColumn + 1].Value.ToString();
                        object dob = workSheet.Cells[startRow, startColumn + 3].Value;
                        if (data != null)
                        {
                            Student stu = db.Students.SingleOrDefault(x => x.UserName.Trim().Equals(userName.Trim()));
                            if(stu.DateOfBirth == null)
                            {
                                stu.DateOfBirth = DateTime.Parse(dob.ToString());
                            }
                            if (stu.StudentCode == null)
                            {
                                stu.StudentCode = userName;
                            }
                            StudentDetail stuDetail = new StudentDetail();
                            stuDetail.StudentID = stu.StudentID;
                            stuDetail.SubjectID = subID;
                            stuDetail.TeacherID = teacherID;
                            db.StudentDetails.Add(stuDetail);
                            db.SaveChanges();
                            result = true;
                        }
                        startRow++;
                    } while (data != null);
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
