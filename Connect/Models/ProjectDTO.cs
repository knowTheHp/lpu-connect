using System.ComponentModel.DataAnnotations;

namespace Connect.Models {
    [MetadataType(typeof(ProjectDTO))]
    public partial class Project {
        public Project() {

        }
        public Project(Project projectModel) {
            ProjectId = projectModel.ProjectId;
            ProjectName = projectModel.ProjectName;
            ProjectDescription = projectModel.ProjectDescription;
            ProjectUrl = projectModel.ProjectUrl;
            ProjectStartMonth = projectModel.ProjectStartMonth;
            ProjectStartYear = projectModel.ProjectStartYear;
            ProjectOnGoing = projectModel.ProjectOnGoing;
            ProjectEndMonth = projectModel.ProjectEndMonth;
            ProjectEndYear = projectModel.ProjectEndYear;
        }
    }
    public class ProjectDTO {
        public int ProjectId { get; set; }
        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project title is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string ProjectName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project description is required")]
        [Display(Name = "Description")]
        [RegularExpression(@"^[0-9 a-z A-Z\.|@_-]+$", ErrorMessage = "invalid character(s) detected.")]
        public string ProjectDescription { get; set; }
        [Display(Name = "Project URL")]
        [DataType(DataType.Url, ErrorMessage = "Enter a valid URL")]
        public string ProjectUrl { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project start month is required")]
        [Display(Name = "Start Month")]
        public int ProjectStartMonth { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project description is required")]
        [Display(Name = "Start Year")]
        public int ProjectStartYear { get; set; }
        [Display(Name = "Project OnGoing")]
        public bool ProjectOnGoing { get; set; }
        [Display(Name = "End Month")]
        public int? ProjectEndMonth { get; set; }
        [Display(Name = "End Year")]
        public int? ProjectEndYear { get; set; }
        public long? UserId { get; set; }
    }
}