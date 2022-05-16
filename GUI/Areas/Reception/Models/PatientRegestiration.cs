using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Views;
using System.Collections.Generic;

namespace GUI.Areas.Reception.Models
{
    public class PatientRegestiration
    {
        public Patient Patient { get; set; }
        public Admission PatientClinic { get; set; }
        public Dictionary<string, Clinic> Clinics { get; set; }
        public string Search { get; set; }
    }
    public class PatientSearch
    {
        public Dictionary<string, Patient> Patients { get; set; }
        public Dictionary<long, PatientRegestiration> PatientRegestiration { get; set; }
        public Dictionary<string, PatientsFollowUps> PatientsFollowUps { get; set; }
        public string Search { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
