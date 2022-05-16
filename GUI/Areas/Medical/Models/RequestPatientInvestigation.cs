using GUI.Models.Views;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GUI.Areas.Medical.Models
{
    public class RequestPatientInvestigation
    {
        public List<string> Types { get; set; }
        [Required(ErrorMessage = "please Choose any Investigation for the Patient")]
        public Dictionary<long, PatientsInvestigations> Investigation { get; set; }
        public string Note { get; set; }
        public long AdmessionID { get; set; }
    }
}
