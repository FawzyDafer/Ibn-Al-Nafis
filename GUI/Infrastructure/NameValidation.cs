using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class NameValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "الأسم لازم يكون رباعي";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {

            var name = context.Model.ToString();
            var names = name.Split(' ');
            if (names.Length < 4)
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

    }
}
