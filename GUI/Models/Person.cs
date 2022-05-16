using GUI.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Please enter the name")]
        [NameValidation]
        public string Name { get; set; }
        [Required(ErrorMessage = "please enter ssn")]
        [RegularExpression(@"(2|3)[0-9][0-9](01|02|03|04|05|06|07|08|09|10|11|12)[0-3][0-9](01|02|03|04|11|12|13|14|15|16|17|18|19|21|22|23|24|25|26|27|28|29|31|32|33|34|35|88)\d\d\d\d\d", ErrorMessage = "SSN is not correct")]
        [SSNValidationModel]
        public string SSN { get; set; }
        [Required(ErrorMessage = "Please select the sex")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Please enter the phone number")]
        [RegularExpression(@"(01)(0|1|2|5)[0-9]{8}", ErrorMessage = "It is not a correct phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter the address")]
        [AddressValidation]
        public string Address { get; set; }
    }
}
