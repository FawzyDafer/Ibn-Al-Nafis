using GUI.Models.Entities;
using System.Collections.Generic;

namespace GUI.Areas.Laps.Models
{
    public class EditInvestigation
    {
        public Investigation Investigation { get; set; }
        public List<InvestigationCategory> Categories { get; set; }
    }
}
