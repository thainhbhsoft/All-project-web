

namespace UET_BTL_VERSION_1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentDetail()
        {
            this.Survey = new HashSet<Survey>();
        }
    
        public int StudentDetailID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public Nullable<int> TeacherID { get; set; }
        public string NoteSurvey { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Survey> Survey { get; set; }
    }
}
