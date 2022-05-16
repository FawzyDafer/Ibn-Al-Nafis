using GUI.Models;
using GUI.Models.Views;
using System.Collections.Generic;

namespace GUI.Areas.Laps.Models
{
    public class SearchInvestigation
    {
        public Dictionary<long, PatientsInvestigations> Investigations { get; set; }
        public string Search { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
