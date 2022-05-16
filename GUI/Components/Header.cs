using GUI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Components
{
    public class Header : ViewComponent
    {
        #region Private Variables
        readonly UserManager<User> _userManager;
        #endregion

        public Header(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync() =>
            View(await CurrentUser);

        #region Private Methods
        private Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion
    }
}

