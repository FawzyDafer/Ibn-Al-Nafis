using System;

namespace GUI.Models.Views
{
    public class PatientsMedicines
    {
        public string MedicineID { set; get; }
        public long AdmissionID { set; get; }
        public bool Checked { get; set; }
        public string PatientsSSN { set; get; }
        public DateTime VisitDate { set; get; }
        public string AlternativeMedicineID { set; get; }
        public string EffectiveMaterial { set; get; }
        public string DoctorID { set; get; }
        public string Medicine { set; get; }
        public string Dose { set; get; }
        public string Frequency { set; get; }
        public string ReasonIfKnown { set; get; }
        public bool Continue { set; get; }
    }
}
