using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.BusinessModel
{
    public class StudentDetailBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Khởi tạo và lưu một studentdetail
        public static void CreateAndSaveStudentDetail(int StudentID, int SubjectID, int TeacherID)
        {
            // Khởi tạo đối tượng sinh viên chi tiết theo môn học
            StudentDetail stuDetail = new StudentDetail();
            stuDetail.StudentID = StudentID;
            stuDetail.SubjectID = SubjectID;
            stuDetail.TeacherID = TeacherID;
            // Lưu một sinh viên vào danh sách học phần
            db.StudentDetails.Add(stuDetail);
            db.SaveChanges();
        }

        // Lấy ra thông tin chi tiết của sinh viên đầu tiên trong học phần đó
        public static StudentDetail GetFirstStudentDetailBySubject(int? id)
        {
            StudentDetail stu = db.StudentDetails.FirstOrDefault(s => s.SubjectID == id);
            if (stu != null)
            {
                return stu;
            }
            return null;
        }

        // Lấy ra thông tin chi tiết của các sinh viên  trong học phần đó
        public static List<StudentDetail> GetAllStudentDetailBySubject(int? id)
        {
            List<StudentDetail>  list = db.StudentDetails.Where(s => s.SubjectID == id).ToList();
            if (list != null)
            {
                return list;
            }
            return null;
        }
    }
}
