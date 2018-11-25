using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.BusinessModel
{
    public class TeacherBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Lấy id của giảng viên theo tên
        public static dynamic GetIdTeacherByName(string name)
        {
            Teacher tea = db.Teachers.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
            if (tea != null)
            {
                return tea.TeacherID;
            }
            return null;
        }
    }
}
