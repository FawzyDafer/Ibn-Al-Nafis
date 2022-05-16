using GUI.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GUI.Areas.Laps.Models
{
    public class EditPatientsInvestigations
    {
        public string PIID { get; set; }
        public long AdmessionID { get; set; }
        public string Type { get; set; }
        public DateTime RequestDate { get; set; }
        public string Note { get; set; }
        public DateTime? FiniahDate { get; set; }
        public string StaffID { get; set; }
        [Mustcheck(ErrorMessage = "You must take the samble")]
        public bool Finish { get; set; }
    }
    public class EditPatientInvestigation : EditPatientsInvestigations
    {
        [Required(ErrorMessage = "Please add the Result", AllowEmptyStrings = false)]
        public string Result { get; set; }
        public string Comment { get; set; }
    }
    public class EditPatientRay : EditPatientsInvestigations
    {
        public string Result { get; set; }
        [Required(ErrorMessage = "Please add the Report", AllowEmptyStrings = false)]
        [Display(Name = "Report")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Please add Ray Image.")]
        [ListImageValidation(ErrorMessage = "Please add Ray Image.")]
        public List<IFormFile> RayImages { get; set; }
    }
}
