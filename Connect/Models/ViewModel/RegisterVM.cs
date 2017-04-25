using System;
using System.ComponentModel.DataAnnotations;

namespace Connect.Models.ViewModel {
    public class RegisterVM {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage = "only characters allowed")]
        [StringLength(maximumLength: 50, ErrorMessage = "length cannot be greater than 50")]
        public string Firstname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage = "only characters allowed")]
        [StringLength(maximumLength: 50, ErrorMessage = "length cannot be greater than 50")]
        public string Lastname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        //[RegularExpression(pattern: "(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Username must contain minimum of 5 characters")]
        [StringLength(maximumLength: 100, ErrorMessage = "length cannot be greater than 100")]
        [System.Web.Mvc.Remote("ValidateUsername", "Account", ErrorMessage = "Username already taken")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email format invalid")]
        [StringLength(maximumLength: 200, ErrorMessage = "length cannot be greater than 200")]
        [System.Web.Mvc.Remote("ValidateEmail", "Account", ErrorMessage = "Email already in use")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage = "Characters only")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [RegularExpression(pattern: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "password must contain at least 8 characters, one digit and one special character")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, ErrorMessage = "length cannot be greater than 200")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }

        [Display(AutoGenerateField = false, Name = "Registration Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Registration number/Employee Id required")]
        public int LpuId { get; set; }

        [Display(AutoGenerateField = false, Name = "Department")]
        public Nullable<int> Department { get; set; }

        [Display(AutoGenerateField = false, Name = "Title")]
        public string Title { get; set; }
        [Display(AutoGenerateField = false, Name = "From")]
        public string FromYear { get; set; }
        [Display(AutoGenerateField = false, Name = "From")]
        public string ToYear { get; set; }
        [Display(AutoGenerateField = false, Name = "Currently Working")]
        public Nullable<bool> CurrentlyWorking { get; set; }

        [Display(AutoGenerateField = false, Name = "Course")]
        public Nullable<int> Course { get; set; }

        [Display(AutoGenerateField = false, Name = "Branch")]
        public Nullable<int> Branch { get; set; }

        [Display(AutoGenerateField = false, Name = "Entry Year")]
        public string EntryYear { get; set; }

        [Display(AutoGenerateField = false, Name = "Graduate Year")]
        public string GraduateYear { get; set; }

        public Nullable<long> UserId { get; set; }
    }
}