using System.ComponentModel.DataAnnotations;

namespace Connect.Models {
    [MetadataType(typeof(SkillsDTO))]
    public partial class UserSkills {
        public UserSkills() {
        }
        public UserSkills(UserSkills userSkills) {
            SkillId = userSkills.SkillId;
            UserId = userSkills.UserId;
        }
    }
    public class SkillsDTO {
        public long UserSkillId { get; set; }
        [Display(Name ="Add Skill")]
        public long SkillId { get; set; }
        public long? UserId { get; set; }
    }
}