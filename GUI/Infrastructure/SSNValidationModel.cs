using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class SSNValidationModel : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "ssn is not correct";
        public int Year { get; set; } = 0;
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            string value = context.Model.ToString();
            var date = MakedatefromSSN(value);
            if (date > DateTime.Now || string.IsNullOrEmpty(value))
            {
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
            else if (date is null)
            {
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
            else
            {
                return Enumerable.Empty<ModelValidationResult>();
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

