using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Authority;
using UET_BTL.Model.BusinessModel;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.Admin.Controllers
{
    [AuthorizeBusiness]
    public class SubjectManagerController : Controller
    {
        // Khởi tạo DBcontext để thao tác với csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();

        // Hiển thị danh sách các học phần
        public ActionResult Index()
        {
                return View(db.Subjects.ToList());
        }

        // Xóa học phần theo id
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra học phần theo id 
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Truyền dữ liệu ra view để xác nhận xóa
            return Json(subject, JsonRequestBehavior.AllowGet);
        }

        // Xác nhận xóa học phần theo id
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra học phần theo id
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Xóa học phần
            db.Subjects.Remove(subject);
            // Lấy ra danh sách các học sinh trong học phần đó
            IEnumerable<StudentDetail> list = StudentDetailBusinessModel.GetAllStudentDetailBySubject(id);
            // xóa các survey của các học sinh đó
            foreach (var item in list)
            {
                try
                {
                    db.Surveys.RemoveRange(item.Survey);
                }
                catch (Exception)
                {

                }
            }
            // Xóa các sinh viên khỏi lớp học phần đó
            db.StudentDetails.RemoveRange(list);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        // Import danh sách sinh viên của một học phần
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

        // Import dữ liệu 
        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                // Bắt đầu từ cột 1
                int startColumn = 1;
                // Bắt đầu từ hàng 12
                int startRow = 12;
                // Lấy sheet 1 của file excel
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                // Khởi tạo DBcontext đẻ thao tác csdl
                UetSurveyDbContext db = new UetSurveyDbContext();
                // Lấy các thông tin của học phần
                object teacherName = workSheet.Cells[7, 3].Value;
                object subjectName = workSheet.Cells[10, 3].Value;
                object subjectCode = workSheet.Cells[9, 3].Value;
                object classRoom = workSheet.Cells[8, 6].Value;
                object creditNumber = workSheet.Cells[9, 6].Value;
                object time = workSheet.Cells[8, 3].Value;
                // Kiểm tra xem học phần đó đã tồn tại chưa
                if (!SubjectBusinessModel.CheckBySubjectCode(subjectCode.ToString()))
                {
                    int creditnumber = int.Parse(creditNumber.ToString());

                    // Tạo và lưu một môn học
                    SubjectBusinessModel.CreateAndSaveSubject(subjectName.ToString(), subjectCode.ToString(), 
                    classRoom.ToString(), creditnumber, time.ToString());

                    // Lấy id của học phần vừa thêm
                    int subID = SubjectBusinessModel.GetLastId();
                    // Lấy ID của giáo viên
                    int teacherID = TeacherBusinessModel.GetIdTeacherByName(teacherName.ToString());

                    do
                    {
                        data = workSheet.Cells[startRow, startColumn].Value;
                        string userName = workSheet.Cells[startRow, startColumn + 1].Value.ToString();
                        object dob = workSheet.Cells[startRow, startColumn + 3].Value;
                        // Kiểm tra xem đã cuối danh sách chưa
                        if (data != null)
                        {
                            // Lấy ra sinh viên có username bằng username trong excel
                            Student stu = StudentBusinessModel.GetStudentByUserName(userName);
                            // Kiểm tra thông tin ngày sinh và cập nhật cho sinh viên
                            if (stu.DateOfBirth == null)
                            {
                                stu.DateOfBirth = DateTime.Parse(dob.ToString());
                            }
                            // Kiểm tra mã sinh viên và cập nhật cho sinh viên
                            if (stu.StudentCode == null)
                            {
                                stu.StudentCode = userName;
                            }
                            // tạo và lưu một studentdetail
                            StudentDetailBusinessModel.CreateAndSaveStudentDetail(stu.StudentID, subID, teacherID);
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

        // Hiển thị kết quả đánh giá theo cả lớp học phần
        public ActionResult ShowResultSurvey(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy số lượng sinh viên đã đánh giá học phần
            ViewBag.hasSurvey = SubjectBusinessModel.GetSumStudentDoneSurvey(id);
            // Lấy ra thông tin chi tiết của sinh viên đầu tiên trong học phần đó
            StudentDetail student_Detail = StudentDetailBusinessModel.GetFirstStudentDetailBySubject(id);
            if (student_Detail == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            if (ViewBag.hasSurvey == 0)
            {
                return View(student_Detail);
            }
            // Lấy tổng số sinh viên của học phần đó
            ViewBag.SumStudent = SubjectBusinessModel.GetSumStudentBySubject(id);
            // Lấy ra điểm trung bình theo cả lớp của các tiêu chí
            ViewBag.ListPointAver = SubjectBusinessModel.GetListAverage(id);
            // Lấy ra danh sách các tiêu chí đánh giá
            ViewBag.NameSurvey = ContentSurveyBusinessModel.GetListContentSurvey();
            // Lấy ra tổng số tiêu chí đánh giá
            ViewBag.CountSurvey = ContentSurveyBusinessModel.GetSumContentSurvey();
            return View(student_Detail);
        }

        // Hiển thị danh sách sinh viên theo học phần
        public ActionResult ShowClass(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra danh sách sinh viên của học phần rồi truyền qua view
            IEnumerable<StudentDetail> listStudent = StudentDetailBusinessModel.GetAllStudentDetailBySubject(id);
            if (listStudent == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            return View(listStudent);
        }

        // Hiển thị kết quả đánh giá của từng sinh viên với học phần tương ứng
        public ActionResult ResultSurveyEveryStudent(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Kiểm tra xem sinh viên đã đánh giá chưa
            ViewBag.hasSurvey = SurveyBusinessModel.GetSumSurveyByStudent(id);
            if (ViewBag.hasSurvey == 0)
            {
                return View();
            }
            // Lấy ra các kết quả đánh giá của sinh viên về học phần đó
            ViewBag.ListPoint = SurveyBusinessModel.GetListPointByStudent(id);
            if (ViewBag.ListPoint == null)
            {
                return RedirectToAction("NotFoundWebsite", "Home", new { area = "SignIn" });
            }
            // Lấy ra các tiêu chí đánh giá để hiển thị trong view
            ViewBag.NameSurvey = ContentSurveyBusinessModel.GetListContentSurvey();
            // Lấy tổng số tiêu chí
            ViewBag.CountSurvey = ContentSurveyBusinessModel.GetSumContentSurvey();
            // Lấy ra  phản hồi của sinh viên về học phần đó
            ViewBag.note = SurveyBusinessModel.GetNoteSurveyByStudent(id);
            return View();
        }

    }
}