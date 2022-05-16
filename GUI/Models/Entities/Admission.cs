using System;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class Admission
    {
        public long Counter { get; set; }
        public string PatientsSSN { get; set; }
        public DateTime VisitDate { get; set; }
        public bool Emergency { get; set; }
        [Required(ErrorMessage = "Please choose the Kind of the ticket")]
        [Display(Name = "Ticket type")]
        public string State { get; set; }
        [Required(ErrorMessage = "Please choose the clinic")]
        public string Clinic { get; set; }
        public bool Isworking { get; set; }
    }
}
