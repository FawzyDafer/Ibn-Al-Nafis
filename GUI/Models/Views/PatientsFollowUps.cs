using System;

namespace GUI.Models.Views
{
    public class PatientsFollowUps
    {
        public string FlowUpID { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string Clinic { get; set; }
        public string NameInArabic { get; set; }
        public string DoctorID { get; set; }
        public DateTime VisitDate { get; set; }
        public long AdmisssionID { get; set; }
        public DateTime FollowUpBegin { get; set; }
        public DateTime FollowUpEnd { get; set; }
        public DateTime FollowUp { get; set; }
        public string FllowupReason { get; set; }
        public int Age
        {
            get { return new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1; }
        }
        public bool Attend { get; set; }
    }
}
