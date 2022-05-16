using System;

namespace GUI.Models.Views
{
    public class PatientsInvestigations
    {
        public string PatientsSSN { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Clinic { get; set; }
        public long AdmessionID { get; set; }
        public string PIID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
        public string Comment { get; set; }
        public DateTime FiniahDate { get; set; }
        public string StaffID { get; set; }
        public bool Finish { get; set; }
        public string FileID { get; set; }
        public byte[] FileData { get; set; }
        public string FileExtention { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Sort { get; set; }
        public string Note { get; set; }
        public string TypeID { get; set; }
    }
}
