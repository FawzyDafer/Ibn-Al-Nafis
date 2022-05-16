using System;

namespace GUI.Models.Entities
{
    public class PatientBloodBank
    {
        public string DonnerID { get; set; }
        public long AdmissionID { get; set; }
        public string DoctorID { get; set; }
        public DateTime Datetime { get; set; }
        public int Amount { get; set; }
        public int NumberOfTimes { get; set; }
    }
}
