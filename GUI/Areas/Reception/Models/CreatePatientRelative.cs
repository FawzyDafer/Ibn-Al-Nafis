using GUI.Models;
using GUI.Models.Entities;
using System.Collections.Generic;

namespace GUI.Areas.Reception.Models
{
    public class CreatePatientRelative
    {
        public Person Relative { get; set; }
        public PatientRelative PatientRelative { get; set; }
    }
    public class GetRelative
    {
        public Dictionary<string, CreatePatientRelative> Companies { get; set; }
        public string SSN { get; set; }
        public long AdmissionID { get; set; }
    }
}
