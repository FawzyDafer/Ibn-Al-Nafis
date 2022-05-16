using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class InvestigationCategory
    {
        public string CategoryID { get; set; }
        [Required(ErrorMessage = "Please Add category Name", AllowEmptyStrings = false)]
        public string Category { get; set; }
        public string Sort { get; set; }
    }
}
