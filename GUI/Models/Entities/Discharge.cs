using System;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class Discharge
    {
        public long AdmissionID { get; set; }
        public bool ISDischarge { get; set; }
        public DateTime DateTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add Patient discharge state")]
        public string Statee { get; set; }
        public string DischargeSummary { get; set; }
        public string DoctorID { get; set; }
    }
}
