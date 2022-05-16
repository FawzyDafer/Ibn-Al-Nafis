using GUI.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class Patient : Person
    {
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please choose the date of birth")]
        [PatientDateValidation(0, "SSN")]
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public int Age
        {
            get
            {
                try
                {
                    return new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public string MariedStatus { get; set; }
        public bool HasChildren { get; set; }
        public string WorkingStatus { get; set; }
        [Required(ErrorMessage = "Please choose patinet Language")]
        public string Language { get; set; }
        [Display(Name = "Required Translator")]
        public bool RequiredTranslator { get; set; }
    }
}
