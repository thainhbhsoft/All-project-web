using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.BusinessModel
{
    public class StudentBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Lấy ra sinh viên có username bằng username truyền vào
        public static Student GetStudentByUserName(string username)
        {
            Student stu =  db.Students.FirstOrDefault(x => x.UserName.Trim().Equals(username.Trim()));
            if (stu != null)
            {
                return stu;
            }
            return null;
        }
    }
}
