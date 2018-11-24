using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Areas.SignIn.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo đối tượng DBcontext để thao tác csdl
        private UetSurveyDbContext db = new UetSurveyDbContext();
       
        // Hiển thị trang đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Areas/SignIn/Views/Home/Login.cshtml");
        }

        // Submit đăng nhập từ form
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // Lấy username từ form
            string username = form["username"].ToString().Trim();
            // Lấy mật khẩu từ form
            string password = form["password"].ToString().Trim();
            // Lấy user có username và password trùng với form submit
            User user = db.Users.FirstOrDefault(x => x.UserName.Equals(username) && x.PassWord.Equals(password));
            // Kiểm tra xem user có tồn tại không
            if (user != null)
            {
                // Gán user vào sesssion
                Session["user"] = user;
               
                // Kiểm tra loại user để chuyển hướng
                if (user.Position.Equals("Teacher"))
                {
                    // Lấy ra giảng viên theo id
                    Teacher teacher = db.Teachers.FirstOrDefault(x => x.TeacherID == user.TeacherID);
                    // Lưu tên user vào session
                    Session["fullName"] = teacher.Name.ToUpper();
                    // CHuyển hướng đến trang chủ cảu giảng viên
                    return RedirectToAction("Index", "Teacher", new { area = "Member" });
                }
                else if (user.Position.Equals("Student"))
                {
                    // Lấy ra sinh viên theo id
                    Student stu = db.Students.FirstOrDefault(x => x.StudentID == user.StudentID);
                    // Lưu tên sinh viên vào session
                    Session["fullName"] = stu.Name.ToUpper();
                    // Chuyển hướng đến trang chủ của sinh viên
                    return RedirectToAction("Index", "Student",new { area = "Member" });
                }
                else
                {
                    // Lưu tên admin vào session
                    Session["fullName"] = "Admin";
                    // Chuyển hướng đến trang chủ của admin
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            return View();
        }

        // Đăng xuất tài khoản người dùng
        public ActionResult Logout()
        {
            // Hủy session lưu user và tên
            Session.Remove("user");
            Session.Remove("fullname");
            // Chuyển hướng đến trang login
            return RedirectToAction("Login", "Home", new { area = "SignIn" });
        }

        // Lấy ra menu tương ứng với từng layout của user
        public PartialViewResult MenuPartial()
        {
            // Lấy user từ session
            User user = Session["user"] as User;
            // Kiểm tra user có tồn tại không
            if (user != null)
            {
                ViewBag.position = user.Position.ToString();
            }
            return PartialView();
        }

        // Hiển thị thông báo lỗi khi không được quyền truy cập
        public ActionResult Authorize()
        {
            return View();
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
