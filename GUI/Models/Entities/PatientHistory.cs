namespace GUI.Models.Entities
{
    public class PatientHistory
    {
        public long AdmissionID { get; set; }
        public string HistoryID { get; set; }
        public string DoctorID { get; set; }
        public string Problem { get; set; }
        public string Note { get; set; }
    }
}
