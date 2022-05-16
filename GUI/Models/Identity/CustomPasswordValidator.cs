using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Models.Identity
{
    public class CustomPasswordValidator : PasswordValidator<User>
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<User> manager, User user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager,
            user, password);
            List<IdentityError> errors = result.Succeeded ?
            new List<IdentityError>() : result.Errors.ToList();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "Password Contains UserName",
                    Description = "Password cannot contain username"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());
        }
    }
}
