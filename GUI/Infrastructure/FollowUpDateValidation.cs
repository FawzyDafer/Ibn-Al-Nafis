using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class FollowUpDateValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "You must add date at the Future";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            DateTime? value = context.Model as DateTime?;
            int? remindYear = value.Value.Year - DateTime.Now.Year;
            var remind = value.Value - DateTime.Now;
            if (value.Value < DateTime.Now || !remindYear.HasValue)
            {
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
            else if (remind.Days >= 31)
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("","Please select aresonable date to appoint follow up in 31 days no more")
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
