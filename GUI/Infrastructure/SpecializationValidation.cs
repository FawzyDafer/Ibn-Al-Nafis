using System.ComponentModel.DataAnnotations;

namespace GUI.Infrastructure
{
    public class SpecializationValidation : ValidationAttribute
    {
        bool IsRequired => false;
        string ComparisonAttribute { get; set; }

        public SpecializationValidation(string staff) =>
            ComparisonAttribute = staff;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Specialization = value?.ToString();

            var property = validationContext.ObjectType.GetProperty(ComparisonAttribute);
            string Staff = property.GetValue(validationContext.ObjectInstance).ToString();

            if (Staff == "Doctor" || Staff == "Head quarter" || Staff == "Manager")
            {
                if (string.IsNullOrEmpty(Specialization))
                {
                    return new ValidationResult("The Doctor must have specialization.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Specialization))
                {
                    return new ValidationResult("The Doctor only could have specialization.");
                }
                return ValidationResult.Success;
            }
        }

    }
}
