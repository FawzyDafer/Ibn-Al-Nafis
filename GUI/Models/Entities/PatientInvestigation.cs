using System;

namespace GUI.Models.Entities
{
    public class PatientInvestigation
    {
        public string PIID { get; set; }
        public long AdmessionID { get; set; }
        public string DoctorID { get; set; }
        public string Type { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
        public string Comment { get; set; }
        public string Note { get; set; }
        public DateTime? FiniahDate { get; set; }
        public string StaffID { get; set; }
        public bool Finish { get; set; }
    }
}
