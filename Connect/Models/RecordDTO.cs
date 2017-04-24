using System;
using System.ComponentModel.DataAnnotations;

namespace Connect.Models {

    [MetadataType(typeof(RecordDTO))]
    public partial class Record {

    }

    public class RecordDTO {
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