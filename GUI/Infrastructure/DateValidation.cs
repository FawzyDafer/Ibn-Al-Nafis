using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class DateValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "You must add date at the past";
        public int Year { get; set; } = 0;

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            DateTime? value = context.Model as DateTime?;
            int? remindYear = DateTime.Now.Year - value.Value.Year;
            if (value.Value > DateTime.Now || !remindYear.HasValue || remindYear.Value < Year)
            {
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
            else if (remindYear.Value >= 60)
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("","You can not add an employee who is older than or equal to 60 years")
                };
            }
            else if (value.Value < DateTime.Parse("1/1/1753"))
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("","You must select a date greater than 1753")
                };
            }
            else
            {
                return Enumerable.Empty<ModelValidationResult>();
            }
        }

    }
}
