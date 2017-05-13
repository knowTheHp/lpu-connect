using System.ComponentModel.DataAnnotations;
namespace Connect.Models {
    [MetadataType(typeof(AwardDTO))]
    public partial class Award {
        public Award() {

        }
        public Award(Award award) {
            AwardId = award.AwardId;
            Name = award.Name;
            Issuer = award.Issuer;
            Description = Description;
            AwardMonth = award.AwardMonth;
            AwardYear = award.AwardYear;
            UserId = award.UserId;
        }
    }

    public class AwardDTO {
        public long AwardId { get; set; }
        [RegularExpression("^[A-Z a-z]+$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Cerificate name is required")]
        [Display(Name="Certificate")]
        public string Name { get; set; }
        [Display(Name="Issuer")]
        [RegularExpression(@"^[0-9 a-z A-Z\.|@_-]+$", ErrorMessage = "invalid character(s) detected.")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Issuer field is required")]
        public string Issuer { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description field is required")]
        [RegularExpression(@"^[0-9 a-z A-Z\.|@_-]+$", ErrorMessage = "invalid character(s) detected.")]
        public string Description { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Month field is required")]
        public int? AwardMonth { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Year field is required")]
        public int? AwardYear { get; set; }
        public long? UserId { get; set; }
    }
}