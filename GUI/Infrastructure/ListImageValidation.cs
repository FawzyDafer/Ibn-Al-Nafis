using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class ListImageValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "You must add image.";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (context.Model is List<IFormFile> value)
            {
                if (value.Count > 0)
                {
                    return Enumerable.Empty<ModelValidationResult>();
                }
                else
                {
                    return new List<ModelValidationResult>
                    {
                        new ModelValidationResult("",ErrorMessage)
                    };
                }
            }
            else
            {
                return new List<ModelValidationResult>
                {
                        new ModelValidationResult("",ErrorMessage)
                };
            }
        }

    }
}
