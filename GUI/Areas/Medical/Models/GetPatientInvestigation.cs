using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Views;
using System.Collections.Generic;

namespace GUI.Areas.Medical.Models
{
    public class GetPatientInvestigation
    {
        public Dictionary<long, PatientsInvestigations> Investigations { get; set; }
        public long Admission { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public PatientsInvestigations PatientInvestigation { get; set; }
        public PatientInvestigation Investigation { get; set; }
        public Dictionary<string, InvestigationFile> Files { get; set; }
    }
}
