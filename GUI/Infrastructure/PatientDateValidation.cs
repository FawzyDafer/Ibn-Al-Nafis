using System;
using System.ComponentModel.DataAnnotations;

namespace GUI.Infrastructure
{
    class PatientDateValidation : ValidationAttribute
    {
        bool IsRequired => true;
        int Year { get; set; } = 0;
        string ComparisonAttribute { get; set; }

        public PatientDateValidation(int year, string ssn)
        {
            Year = year;
            ComparisonAttribute = ssn;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(ComparisonAttribute);
            var SSNValue = property.GetValue(validationContext.ObjectInstance).ToString();
            var combarisondate = MakedatefromSSN(SSNValue);
            int? remindYear = DateTime.Now.Year - currentValue.Year;
            if (currentValue > DateTime.Now || !remindYear.HasValue || remindYear.Value < Year)
            {
                return new ValidationResult("يجب أضافة تاريخ ميلاد في الماضي");
            }
            else if (currentValue < DateTime.Parse("1/1/1753"))
            {
                return new ValidationResult("يجب أختيار تاريخ أكبر من  سنة 1753");
            }
            else if (combarisondate != currentValue)
            {
                return new ValidationResult("الرقم القومي او تاريخ الميلاد غير صحيح");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        DateTime? MakedatefromSSN(string value)
        {
            int year = 0;
            string t1 = value[1].ToString();
            string t2 = value[2].ToString();
            if (value[0] == '2')
            {
                year = 1900 + Convert.ToInt32(t1) * 10 + Convert.ToInt32(t2);
            }
            else if (value[0] == '3')
            {
                year = 2000 + int.Parse(t1) * 10 + int.Parse(t2);
            }
            t1 = value[3].ToString();
            t2 = value[4].ToString();
            int month = int.Parse(t1) * 10 + int.Parse(t2);
            t1 = value[5].ToString();
            t2 = value[6].ToString();
            int day = Convert.ToInt32(t1) * 10 + Convert.ToInt32(t2);
            try
            {
                return new DateTime(year, month, day);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

