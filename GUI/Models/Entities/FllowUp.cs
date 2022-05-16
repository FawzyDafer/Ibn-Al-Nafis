using GUI.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class FllowUp
    {
        public string FllowUpID { get; set; }
        [Required(ErrorMessage = "Please Add FllowUp date")]
        [FollowUpDateValidation]
        public DateTime FollowUp { get; set; }
        public DateTime FollowUpBegin { get; set; }
        public DateTime FollowUpEnd { get; set; }
        [Required(ErrorMessage = "Please Add fllow up reason")]
        public string FllowupReason { get; set; }
        public long AdmisssionID { get; set; }
        public string DoctorID { get; set; }
    }
}
