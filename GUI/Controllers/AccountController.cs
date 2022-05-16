using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebGUI.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        #region Private Variables
        readonly IFAQServices FAQServices;
        readonly UserManager<User> _userManager;
        Task<User> CurrentUser =>
           _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constructor
        public AccountController(
            UserManager<User> userManager,
            IFAQServices fAQServices)
        {
            _userManager = userManager;
            FAQServices = fAQServices;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(string personid) =>
            View(await EditUser(personid));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EditUser EUser)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(EUser.Id);
                if (!string.IsNullOrEmpty(EUser.Image))
                {
                    user.ImageData = Convert.FromBase64String(EUser.Image.Split(',')[1]);
                    user.ImageExtention = EUser.Image.Split(':', ';')[1];
                }
                user.PhoneNumber = EUser.Phone;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Sucssess"] = "Data has been successfully modified.";
                }
                else
                {
                    TempData["Errors"] = AddErrorsFromResult(result);
                }
            }
            string Personid = EUser.UserName;
            return RedirectToAction("Index", new { Personid });
        }
        [HttpGet]
        public IActionResult ChangePassword(string personid) =>
            View(new ChangePassword() { UserName = personid });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword ChangePassword)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByNameAsync(ChangePassword.UserName);
                var result = await _userManager.ChangePasswordAsync
                    (User, ChangePassword.OldPassword, ChangePassword.Password);
                if (result.Succeeded)
                {
                    string Search = ChangePassword.Search;
                    TempData["Sucssess"] = "The password has been successfully changed";
                    return RedirectToAction(nameof(AccountController.Index),
                        new { personid = ChangePassword.UserName });
                }
                else
                {
                    TempData["Error"] = AddErrorsFromResult(result);
                }
            }
            return View(ChangePassword);
        }
        [HttpPost]
        public async Task<IActionResult> FAQ(FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                var user = await CurrentUser;
                fAQ.UserID = user.Id;
                var Result = await FAQServices.FAQ_Add(fAQ);
                if (Result.Success)
                {
                    return Ok();
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult AccessDenied() => View();
        [HttpGet]
        public async Task<FileStreamResult> ViewImage(string personid)
        {
            var User = await _userManager.FindByNameAsync(personid);

            MemoryStream ms = new MemoryStream(User.ImageData);

            return new FileStreamResult(ms, User.ImageExtention);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPassword(string username, string Password)
        {
            var User = await _userManager.FindByNameAsync(username);
            var Result = await _userManager.CheckPasswordAsync(User, Password);
            if (Result)
            {
                return Json($"you can not add the same password please add another one.");
            }
            return Json(true);
        }

        #region Private Methods
        async Task<EditUser> EditUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(user);
            return new EditUser
            {
                Id = user.Id,
                DateofBirth = user.DateOfBirth,
                Name = user.Name,
                UserName = user.UserName,
                Sex = user.Gender,
                Staff = role.FirstOrDefault(),
                Qualification = user.Qualification,
                Specialization = user.Specialization,
                Phone = user.PhoneNumber
            };
        }
        List<IdentityError> AddErrorsFromResult(IdentityResult result)
        {
            return result.Errors.ToList();
        }

        #endregion

    }
}