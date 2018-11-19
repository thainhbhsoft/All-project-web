

namespace UET_BTL.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Student
    {
        public int StudentID { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string StudentCode { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string Course { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        //[RegularExpression(@"\d\d\d\d\d\d\d\d@vnu.edu.vn", ErrorMessage = "Email phải có dạng xxxxxxxx@vnu.edu.vn với x là số")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string PassWord { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetail { get; set; }
    }
}
