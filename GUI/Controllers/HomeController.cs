using GUI.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        #region Private Variables
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        #endregion

        #region Constractor
        public HomeController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid User name or Passowrd");
                    return View("Index", model);
                }
                if (user.FirstLogin)
                {
                    return RedirectToAction("Change", "Home", new { uname = model.UserName });
                }
                var Result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,
                    true, lockoutOnFailure: true);
                if (Result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User name or Passowrd");
                }
            }
            return View("Index", model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index));
        }
        [HttpGet]
        public IActionResult About() => View();
        [HttpGet]
        public async Task<IActionResult> Change(string uname)
        {
            var user = await _userManager.FindByNameAsync(uname);
            if (user.FirstLogin)
            {
                return View(new ChangePassword() { UserName = uname });
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Change(ChangePassword ChangePassword)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByNameAsync(ChangePassword.UserName);
                var result = await _userManager.RemovePasswordAsync(User);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(User, ChangePassword.Password);
                    User.FirstLogin = false;
                    await _userManager.UpdateAsync(User);
                    var Result = await _signInManager.PasswordSignInAsync
                        (User.UserName, ChangePassword.Password, true, lockoutOnFailure: true);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = AddErrorsFromResult(result);
                }
            }
            return View(ChangePassword);
        }
        [HttpGet]
        public IActionResult Error() => View();

        #region Private Methods
        List<IdentityError> AddErrorsFromResult(IdentityResult result)
        {
            return result.Errors.ToList();
        }
        #endregion

    }
}