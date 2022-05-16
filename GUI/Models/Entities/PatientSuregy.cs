using System;

namespace GUI.Models.Entities
{
    public class PatientSuregy
    {
        public string PSID { get; set; }
        public string PatientID { get; set; }
        public string SuregyID { get; set; }
        public string DoctorID { get; set; }
        public DateTime Date { get; set; }
        public string Detail { get; set; }
        public string Result { get; set; }
        public bool Postbet { get; set; }
    }
}
