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
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Department Required")]
        public Nullable<int> Department { get; set; }
        [Display(AutoGenerateField = false, Name = "Title")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Title required")]
        public string Title { get; set; }
        [Display(AutoGenerateField = false, Name = "From")]
        //[Required(AllowEmptyStrings = true, ErrorMessage = "From required")]
        public string FromYear { get; set; }
        [Display(AutoGenerateField = false, Name = "From")]
        public string ToYear { get; set; }
        [Display(AutoGenerateField = false, Name = "Currently Working")]
        public Nullable<bool> CurrentlyWorking { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Course required")]
        [Display(AutoGenerateField = false, Name = "Course")]
        public Nullable<int> Course { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Branch required")]
        [Display(AutoGenerateField = false, Name = "Branch")]
        public Nullable<int> Branch { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Entry year required")]
        [Display(AutoGenerateField = false, Name = "Entry Year")]
        public string EntryYear { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Graduate year required")]
        [Display(AutoGenerateField = false, Name = "Graduate Year")]
        public string GraduateYear { get; set; }

        public Nullable<long> UserId { get; set; }
    }
}