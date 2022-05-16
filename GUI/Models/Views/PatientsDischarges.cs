using System;

namespace GUI.Models.Views
{
    public class PatientsDischarges
    {
        public long AdmissionID { set; get; }
        public DateTime DateTime { get; set; }
        public bool Discharge { set; get; }
        public string State { set; get; }
        public string DischargeSummary { get; set; }
        public DateTime VisitDate { get; set; }
        public string PatientsSSN { get; set; }
    }
}
