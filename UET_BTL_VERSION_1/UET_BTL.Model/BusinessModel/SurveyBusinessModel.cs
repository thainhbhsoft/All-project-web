using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.BusinessModel
{
    public class SurveyBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Lấy ra tổng các đánh giá của một học sinh
        public static int GetSumSurveyByStudent(int? studentDetailId)
        {
            return db.Surveys.Where(x => x.StudentDetailID == studentDetailId).ToList().Count();
        }
        
        // Lấy ra các điểm đánh giá của một học sinh
        public static dynamic GetListPointByStudent(int? studentDetailId)
        {
            var list =  db.Surveys
              .Where(x => x.StudentDetailID == studentDetailId)
              .Select(x => x.Point).ToList();
            if (list != null)
            {
                return list;
            }
            return null;
        }

        // Lấy ra phản hồi của một học sinh
        public static string GetNoteSurveyByStudent(int? studentDetailId)
        {
            StudentDetail stu = db.StudentDetails.FirstOrDefault(x => x.StudentDetailID == studentDetailId);
            if (stu != null)
            {
                return stu.NoteSurvey;
            }
            return null;
        }

    }
}
