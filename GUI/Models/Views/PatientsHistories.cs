using System;

namespace GUI.Models.Views
{
    public class PatientsHistories
    {
        public long AdmissionID { set; get; }
        public string HistoryID { set; get; }
        public string Description { set; get; }
        public string Note { set; get; }
        public string PatientsSSN { set; get; }
        public DateTime VisitDate { set; get; }
        public string Clinic { set; get; }
        public bool Emergency { set; get; }
        public string DoctorID { set; get; }
        public string Problem { set; get; }
        public string CategoryID { set; get; }
        public string Category { set; get; }
    }
}
