using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class ImageValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "You must add image.";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (context.Model is IFormFile value)
            {
                if (value.ContentType.ToLower().StartsWith("image/"))
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
                return Enumerable.Empty<ModelValidationResult>();
            }
        }
    }
}