using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class PatientRelative
    {
        public long AdmissionID { get; set; }
        public string RelativeSSN { get; set; }
        [Required(ErrorMessage = "Please select the relation")]
        public string Relation { get; set; }
        public string PatientSSN { get; set; }
    }
}
