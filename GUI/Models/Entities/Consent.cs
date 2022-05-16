using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class Consent
    {
        public string ConID { get; set; }
        [Required(ErrorMessage = "Please add Consent Title", AllowEmptyStrings = false)]
        public string Description { get; set; }
    }
}
