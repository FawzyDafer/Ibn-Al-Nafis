using System;

namespace GUI.Models.Views
{
    public class PatientsRelatives
    {
        public string RelativeSSN { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Relation { get; set; }
        public long AdmissionID { get; set; }
        public string PatientsSSN { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
