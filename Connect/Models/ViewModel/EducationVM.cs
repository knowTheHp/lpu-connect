using System.ComponentModel.DataAnnotations;

namespace Connect.Models.ViewModel {
    public class EducationVM {
        public EducationVM() {

        }

        public EducationVM(Education education) {
            School = education.School;
            DegreeType = education.DegreeType;
            EduFrom = education.EduFrom;
            EduTo = education.EduTo;
            UserId = education.UserId;
        }

        [Key]
        public long EducationId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "School/College is required")]
        [Display(Name = "School/College")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string School { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Degree is required")]
        [Display(Name = "Degree")]
        public int DegreeType { get; set; }
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public string Course { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "From date is required")]
        [Display(Name = "From")]
        public int EduFrom { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "To date is required")]
        [Display(Name = "To")]
        public int EduTo { get; set; }
        public long UserId { get; set; }
    }
}