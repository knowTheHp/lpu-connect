using System.ComponentModel.DataAnnotations;

namespace Connect.Models {
    [MetadataType(typeof(EducationDTO))]
    public partial class Education {
        public Education() {

        }
        public Education(Education educationModel) {
            EducationId = educationModel.EducationId;
            School = educationModel.School;
            DegreeType = educationModel.DegreeType;
            Course = educationModel.Course;
            EduFrom = educationModel.EduFrom;
            EduTo = educationModel.EduTo;
            UserId = educationModel.UserId;
        }
    }
    public class EducationDTO {
        public long EducationId { get; set; }
        [Display(Name = "School/College")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "School/College is required")]
        [RegularExpression(@"^[a-z A-Z._-]+$", ErrorMessage = "invalid character(s) detected.")]
        public string School { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Degree is required")]
        [Display(Name = "Degree")]
        public int DegreeType { get; set; }
        [RegularExpression(@"^[0-9 a-z A-Z-+]+$", ErrorMessage = "invalid character(s) detected.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public string Course { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "From year is required")]
        [Display(Name = "From")]
        public int EduFrom { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "To year is required")]
        [Display(Name = "To")]
        public int EduTo { get; set; }
        public long UserId { get; set; }
    }
}