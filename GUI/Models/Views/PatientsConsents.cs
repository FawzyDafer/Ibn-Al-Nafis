using System;
using System.Collections.Generic;

namespace GUI.Models.Views
{
    public class PatientsConsents
    {
        public string PCID { get; set; }
        public long AdmissionID { get; set; }
        public string PatientsSSN { get; set; }
        public DateTime VisitDate { get; set; }
        public string ConID { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtention { get; set; }
    }
    public class GetPatientConsent
    {
        public Dictionary<long, PatientsConsents> PatientsConsents { get; set; }
        public long Admission { get; set; }
    }
}
