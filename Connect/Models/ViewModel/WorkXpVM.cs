using System.ComponentModel.DataAnnotations;

namespace Connect.Models.ViewModel {
    public class WorkXpVM {

        public WorkXpVM() {

        }

        public WorkXpVM(WorkXp workXp) {
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

        [Key]
        public int WorkxpId { get; set; }
        [Display(Name ="Company")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Company is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string Company { get; set; }
        [Display(Name ="Designation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string Designation { get; set; }
        [Display(Name ="City")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        public string City { get; set; }
        [Display(Name ="Country")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
        public int Country { get; set; }
        [Display(Name ="Start Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "From Month is required")]
        public int FromMonth { get; set; }
        [Display(Name = "Start Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "From Year is required")]
        public int FromYear { get; set; }
        [Display(Name = "To Month")]
        public int? ToMonth { get; set; }
        [Display(Name = "To Year")]
        public int? ToYear { get; set; }
        [Display(Name ="Is Currently Working?")]
        public bool? IsCurrentlyWorking { get; set; }
        public long UserId { get; set; }
    }
}