

namespace UET_BTL.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ContentSurvey
    {
        public int ContentSurveyID { get; set; }
        //[Required(ErrorMessage = "Không được để trống")]
        public string Text { get; set; }
    
        public virtual ICollection<Survey> Survey { get; set; }
    }
}
