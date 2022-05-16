using System.ComponentModel.DataAnnotations;

namespace GUI.Areas.Medical.Models
{
    public class Digonose
    {
        public string ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add patients Examinatios")]
        public string Examination { get; set; }
        public string Clinic { get; set; }
        public string DoctorID { get; set; }
        public long Admission { get; set; }
    }
}
