namespace GUI.Models.Entities
{
    public class PatientExamination
    {
        public string ExaminationID { get; set; }
        public long AdmissionID { get; set; }
        public string DoctorID { get; set; }
        public string Problem { get; set; }
        public string Note { get; set; }
    }
}
