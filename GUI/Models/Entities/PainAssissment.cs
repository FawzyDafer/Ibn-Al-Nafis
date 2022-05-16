using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class PainAssissment
    {
        public string PainID { get; set; }
        [Display(Name = "Does the patient has pain")]
        public bool DoesPatienthaspain { get; set; }
        public long AdmissionID { get; set; }
        public bool HasPain { get; set; }
        [Display(Name = "Rate patient pain")]
        public int Rate { get; set; }
        public string Location { get; set; }
        public string Character { get; set; }
        public string Frequency { get; set; }
        [Display(Name = "Relieves by")]
        public string Relieves { get; set; }
        public string Referral { get; set; }
        public string Duration { get; set; }
        [Display(Name = "Aggravates by")]
        public string Aggravates { get; set; }
        public string PastPain { get; set; }
        [Display(Name = "Impact on dialy activities")]
        public string Impact { get; set; }
        [Display(Name = "Previous therapies")]
        public string Therapies { get; set; }
    }
}
