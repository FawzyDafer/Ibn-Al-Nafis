using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GUI.Models.Identity
{
    public class CustomUserValidator : UserValidator<User>
    {
        public override async Task<IdentityResult>
            ValidateAsync(UserManager<User> manager, User user)
        {
            return await base.ValidateAsync(manager, user);
        }

    }
}
