using GUI.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class PC
    {
        public string PCID { get; set; }
        [Required(ErrorMessage = "Please choose a cnsent", AllowEmptyStrings = false)]
        public string ConID { get; set; }
        public long AdmissionID { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtention { get; set; }
        public Dictionary<string, Consent> Consents { get; set; }
    }
    public class PatientConsent : PC
    {
        [Required(ErrorMessage = "Please add Consent image", AllowEmptyStrings = false)]
        [ImageValidation(ErrorMessage = "Please Choose Image to upload.")]
        public IFormFile Image { get; set; }
    }
    public class EPatientConsent : PC
    {
        [ImageValidation(ErrorMessage = "Please Choose Image to upload.")]
        public IFormFile Image { get; set; }
    }
}
