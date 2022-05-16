using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class ClinicsDay
    {
        public string Clinic { get; set; }
        public int DayID { get; set; }
    }
    public class ClincWorkingDays
    {
        [Required(ErrorMessage = "You must select Clinic", AllowEmptyStrings = false)]
        public string Clinic { get; set; }
        public long? Admission { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
    }

}
