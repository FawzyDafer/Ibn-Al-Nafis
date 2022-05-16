using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Entities
{
    public class Investigation
    {
        public string TypeID { get; set; }
        [Required(ErrorMessage = "Please Add investigation title", AllowEmptyStrings = false)]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please Add Category", AllowEmptyStrings = false)]
        public string CategoryID { get; set; }
        public string Category { get; set; }
        public string Sort { get; set; }
    }
}
