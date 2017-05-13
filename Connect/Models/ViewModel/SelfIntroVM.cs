using System.ComponentModel.DataAnnotations;
namespace Connect.Models.ViewModel {
    public class SelfIntroVM {
        public SelfIntroVM() {

        }

        public SelfIntroVM(SelfIntro intro) {
            Intro = intro.Intro;
            UserId = intro.UserId;
        }
        [Key]
        public long IntroId { get; set; }
        [Display(Name ="Intro")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Intro required")]
        [RegularExpression(@"^[0-9 a-z A-Z\.|@_-]+$",ErrorMessage ="invalid character(s) detected.")]
        public string Intro { get; set; }
        public long? UserId { get; set; }
    }
}