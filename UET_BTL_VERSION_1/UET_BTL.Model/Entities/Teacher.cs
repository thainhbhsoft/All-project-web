
namespace UET_BTL.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Teacher
    {
        public int TeacherID { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        public string TeacherCode { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(@"[a-z]{4,10}@vnu.edu.vn", ErrorMessage = "Email phải có dạng x@vnu.edu.vn với x từ 4 đến 10 các chữ cái")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string PassWord { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetail { get; set; }
    }
}
