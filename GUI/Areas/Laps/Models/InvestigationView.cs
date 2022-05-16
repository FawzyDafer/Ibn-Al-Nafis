using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Models.Views;
using System.Collections.Generic;

namespace GUI.Areas.Laps.Models
{
    public class InvestigationsViews
    {
        public Patient Patient { get; set; }
        public User Doctor { get; set; }
        public Admission Admission { get; set; }
        public PatientsInvestigations PatientInvestigation { get; set; }
        public Dictionary<long, PatientsInvestigations> PatientsInvestigations { get; set; }
    }

    public class InvestigationView : InvestigationsViews
    {
        public EditPatientInvestigation EditInvestigation { get; set; }
    }

    public class RayView : InvestigationsViews
    {
        public EditPatientRay EditRay { get; set; }
    }

    public class SearchView
    {
        public PagingInfo PagingInfo { get; set; }
        public string Search { get; set; }
    }

    public class InvestigationViewSearch : SearchView
    {
        public Dictionary<long, InvestigationView> InvestigationView { get; set; }
    }

    public class RayViewSearch : SearchView
    {
        public Dictionary<long, RayView> InvestigationView { get; set; }
    }

}
