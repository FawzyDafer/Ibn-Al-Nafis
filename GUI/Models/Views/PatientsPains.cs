using System;

namespace GUI.Models.Views
{
    public class PatientsPains
    {
        public long AdmissionID { get; set; }
        public string PatientsSSN { get; set; }
        public DateTime VisitDate { get; set; }
        public bool Emergency { get; set; }
        public string Clinic { get; set; }
        public string PainID { get; set; }
        public bool HasPain { get; set; }
        public int Rate { get; set; }
        public string Location { get; set; }
        public string Character { get; set; }
        public string Frequency { get; set; }
        public string Relieves { get; set; }
        public string Referral { get; set; }
        public string Duration { get; set; }
        public string Aggravates { get; set; }
        public string PastPain { get; set; }
        public string Impact { get; set; }
        public string Therapies { get; set; }
    }
}
