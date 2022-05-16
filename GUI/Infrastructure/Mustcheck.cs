using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class Mustcheck : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "Must be Checked";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            bool Checked = Convert.ToBoolean(context.Model);
            if (Checked == false)
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

