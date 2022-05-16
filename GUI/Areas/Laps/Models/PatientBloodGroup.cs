using System.ComponentModel.DataAnnotations;

namespace GUI.Areas.Laps.Models
{
    public class PatientBloodGroup
    {
        public string SSN { get; set; }
        [Required(ErrorMessage = "Please Choose Blood Group", AllowEmptyStrings = false)]
        public string BloodGroup { get; set; }
    }
}
