using System.ComponentModel.DataAnnotations;

namespace Connect.Models {
    [MetadataType(typeof(WorkXpDTO))]
    public partial class WorkXp {
        public WorkXp() {

        }

        public WorkXp(WorkXp workXp) {
            WorkxpId = workXp.WorkxpId;
            Company = workXp.Company;
            Designation = workXp.Designation;
            City = workXp.City;
            Country = workXp.Country;
            FromMonth = workXp.FromMonth;
            FromYear = workXp.FromYear;
            ToMonth = workXp.ToMonth;
            ToYear = workXp.ToYear;
            IsCurrentlyWorking = workXp.IsCurrentlyWorking;
            UserId = workXp.UserId;
        }
    }

    public class WorkXpDTO {
        public int WorkxpId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string Company { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string Designation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string City { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        public int Country { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        public int FromMonth { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        public int FromYear { get; set; }
        public int? ToMonth { get; set; }
        public int? ToYear { get; set; }
        public bool? IsCurrentlyWorking { get; set; }
        public long UserId { get; set; }
    }
}