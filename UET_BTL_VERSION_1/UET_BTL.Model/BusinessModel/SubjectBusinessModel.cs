using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.BusinessModel
{
    public class SubjectBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Khởi tạo và lưu một subject
        public static void CreateAndSaveSubject(string Name, string subjectCode, string classRoom, int creditnumber,string TimeTeach)
        {
            // Khởi tạo học phần và gán các giá trị cho thuộc tính
            Subject sub = new Subject();
            sub.Name = Name;
            sub.SubjectCode = subjectCode;
            sub.ClassRoom = classRoom;
            sub.CreditNumber = creditnumber;
            sub.TimeTeach = TimeTeach;
            // lưu học phần vào DB
            db.Subjects.Add(sub);
            db.SaveChanges();
        }

        // Lấy độ lệch chuẩn các tiêu chí theo môn học
        public static  List<float> GetStandardDeviationBySubject(int? subjectId, List<int> list)
        {
            return null;
        }

        // Lấy danh sách subjectdetailId theo học phần
        public static List<int> GetListSubjectDetailId(int? Id)
        {
            Subject subject = db.Subjects.FirstOrDefault(x => x.SubjectID == Id);
            if (subject != null)
            {
                return subject.StudentDetail.ToList().Select(x => x.StudentDetailID).ToList();
            }
            return null;
        }

        // Lấy tổng sinh viên đã đánh giá học phần theo môn học
        public static int GetSumStudentDoneSurvey(int? subjectId)
        {
            List<int> lis = GetListSubjectDetailId(subjectId);
            if (lis == null)
            {
                return 0;
            }
            int sum = db.Surveys.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            return sum;
        }

        // Lấy tổng sinh viên của một học phần 
        public static int GetSumStudentBySubject(int? subjectId)
        {
            int sum = db.Subjects.FirstOrDefault(x => x.SubjectID == subjectId).StudentDetail.ToList().Count();
            return sum;
        }
       
        // Lấy giá trị trung bình của các tiêu chí theo môn học
        public static dynamic GetListAverage(int? subjectId)
        {
            List<int> lis = GetListSubjectDetailId(subjectId);
            if (lis == null)
            {
                return null;
            }
            var lis2 = db.Surveys
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            if (lis2 != null)
            {
                return lis2;
            }
            return null;
        }

        // Lấy ID cuối cùng của một học phần 
        public static int GetLastId ()
        {
            return db.Subjects.Max(x => x.SubjectID);
        }

        // Kiểm tra học phần có tồn tại theo mã không
        public static bool CheckBySubjectCode(string subjectCode)
        {
            return db.Subjects.Any(x => x.SubjectCode.ToLower().Equals(subjectCode.ToLower()));
        }
    }
}
