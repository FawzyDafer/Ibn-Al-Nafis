using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Infrastructure
{
    public class AddressValidation : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "at leaset select government and a city";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var address = context.Model.ToString();
            var addresses = address.Split('-');
            if (addresses.Length < 3)
            {
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
            else if (addresses.Length == 3 && string.IsNullOrEmpty(addresses[0]))
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
