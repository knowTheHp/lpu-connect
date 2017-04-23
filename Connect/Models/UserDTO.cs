using System.ComponentModel.DataAnnotations;
namespace Connect.Models {

    [MetadataType(typeof(UserDTO))]
    public partial class User {
        public User(User userModel) {
            UserId = userModel.UserId;
            Firstname = userModel.Firstname;
            Lastname = userModel.Lastname;
            Username = userModel.Username;
            Email = userModel.Email;
            Password = userModel.Password;

        }
    }
    public class UserDTO {
        public long UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage = "only characters allowed")]
        [StringLength(maximumLength: 50, ErrorMessage = "length cannot be greater than 50")]
        public string Firstname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage = "only characters allowed")]
        [StringLength(maximumLength: 50, ErrorMessage = "length cannot be greater than 50")]
        public string Lastname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        [RegularExpression(pattern: "(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Username must contain minimum of 5 characters")]
        [StringLength(maximumLength: 100, ErrorMessage = "length cannot be greater than 100")]
        //[Remote("CheckUsername", "Account", ErrorMessage ="Username already taken")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email format invalid")]
        [StringLength(maximumLength: 200, ErrorMessage = "length cannot be greater than 200")]
        //[Remote("CheckEmail", "Account", ErrorMessage = "Email already in use")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City required")]
        [RegularExpression(pattern: "^[A-Za-z]+$", ErrorMessage ="Characters only")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, ErrorMessage = "length cannot be greater than 200")]
        //[RegularExpression(pattern: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "password must contain at least 8 characters, one digit and one special character")]
        public string Password { get; set; }
    }
}