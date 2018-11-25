using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UET_BTL.Model.BusinessModel
{
    public class ContentSurveyBusinessModel
    {
        // Khởi tạo đối tượng DBcontext để thao tác với CSDL
        public static UetSurveyDbContext db = new UetSurveyDbContext();

        // Lấy ra danh sách các tiêu chí đánh giá
        public static List<string> GetListContentSurvey()
        {
            List<string> list =  db.ContentSurveys.Select(x => x.Text).ToList();
            if (list != null)
            {
                return list;
            }
            return null;
        }

        // Lấy ra tổng các tiêu chí đánh giá
        public static int GetSumContentSurvey()
        {
            return db.ContentSurveys.ToList().Count();
        }
    }
}
