namespace GUI.Models.Entities
{
    public class PatientMedicine
    {
        public long AdmissionID { get; set; }
        public string MedicineID { get; set; }
        public string DoctorID { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public string ReasonIfKnown { get; set; }
        public bool Continue { get; set; }
    }
}
