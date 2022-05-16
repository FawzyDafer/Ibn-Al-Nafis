using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Models.Identity
{
    public class BlockUsersRequirement : IAuthorizationRequirement
    {
        public string[] BlockedUsers { get; set; }

        public BlockUsersRequirement(params string[] users)
        {
            BlockedUsers = users;
        }

    }

    public class BlockUsersHandler : AuthorizationHandler<BlockUsersRequirement>
    {

        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BlockUsersRequirement requirement)
        {
            if (context.User.Identity != null && context.User.Identity.Name != null
            && !requirement.BlockedUsers
            .Any(user => user.Equals(context.User.Identity.Name,
            StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

    }

}