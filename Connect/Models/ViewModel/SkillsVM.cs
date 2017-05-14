using System.ComponentModel.DataAnnotations;

namespace Connect.Models.ViewModel {


    public class SkillsVM {
        public SkillsVM() {

        }
        public SkillsVM(UserSkills userSkills) {
            SkillId = userSkills.SkillId;
            UserId = userSkills.UserId;
        }
        [Key]
        public long SkillsId { get; set; }
        [Display(Name = "Skill One")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Skill required")]
        public long SkillId{ get; set; }
        public long? UserId { get; set; }
    }
}