using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Manager")]
    public class StaffController : Controller
    {
        #region Private Variables
        readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> RoleManager;
        readonly ILogFileServices LogFileServices;
        readonly IImageService ImageService;
        readonly int PageSize = 30;
        readonly string BasePassword = "123456789";
        Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constractor
        public StaffController(
            UserManager<User> userManager,
            ILogFileServices logFileServices,
            IImageService imageService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            LogFileServices = logFileServices;
            RoleManager = roleManager;
            ImageService = imageService;
        }
        #endregion

        public async Task<IActionResult> Index(string Search, int Page = 1) =>
            View(await GetUserwithRoles(Search, Page));
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUser());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUser CreateUser)
        {
            if (ModelState.IsValid)
            {
                var image = (await ImageService.GetImageByCategory("User")).FirstOrDefault();
                string specialies = GetSpecialization(CreateUser.Staff, CreateUser.Specialization);
                var user = new User
                {
                    Name = CreateUser.Name,
                    UserName = CreateUser.UserName,
                    Gender = CreateUser.Sex,
                    DateOfBirth = CreateUser.DateofBirth,
                    Qualification = CreateUser.Qualification,
                    Specialization = specialies,
                    Clinic = specialies,
                    FirstLogin = true,
                    ImageData = image.ImageData,
                    ImageExtention = image.ImageExtention
                };
                IdentityResult result = await _userManager.CreateAsync(user, BasePassword);
                if (result.Succeeded)
                {
                    await AddUserToRole(user, CreateUser.Staff, specialies);
                    var existeduser = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = existeduser.Id,
                        Description = $"{existeduser.Name} add new user to the system his/her name is" +
                        $"{user.Name}, user name {user.UserName} and jop is {CreateUser.Staff}"
                    });
                    TempData["Sucssess"] = "The staff has been added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = AddErrorsFromResult(result);
                }
            }
            return View(CreateUser);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string uname, string Search) =>
            View(await EditUser(uname, Search));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUser EditUser)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(EditUser.Id);
                string specialies = GetSpecialization(EditUser.Staff, EditUser.Specialization);
                user.Name = EditUser.Name;
                user.Gender = EditUser.Sex;
                user.DateOfBirth = EditUser.DateofBirth;
                user.Qualification = EditUser.Qualification;
                user.Specialization = specialies;
                user.Clinic = specialies;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.RemoveFromRolesAsync(user,
                        await _userManager.GetRolesAsync(user));
                    await _userManager.AddToRoleAsync(user, EditUser.Staff);
                    await AddUserToRole(user, EditUser.Staff, specialies);
                    var existeduser = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = existeduser.Id,
                        Description = $"{existeduser.Name} edit existed user data his/her name is" +
                        $"{user.Name}, user name {user.UserName} and jop is {EditUser.Staff}"
                    });
                    TempData["Sucssess"] = "Data has been successfully modified.";
                    int Page = 1;
                    string Search = EditUser.Search;
                    return RedirectToAction("Index", routeValues: new { Search, Page });
                }
                else
                {
                    TempData["error"] = AddErrorsFromResult(result);
                }
            }
            return View(EditUser);
        }

        public async Task<ActionResult> ResetPassword(string uname, string Search)
        {
            User User = await _userManager.FindByNameAsync(uname);
            var Result = await _userManager.RemovePasswordAsync(User);
            if (Result.Succeeded)
            {
                await _userManager.AddPasswordAsync(User, BasePassword);
                User.FirstLogin = true;
                await _userManager.UpdateAsync(User);
                var existeduser = await CurrentUser;
                await LogFileServices.AddLogFile(new LogFile
                {
                    UserID = existeduser.Id,
                    Description = $"{existeduser.Name} reset existed user his/her password" +
                    $"and his/her user name {uname}, name is {User.Name}"
                });
                TempData["Sucssess"] = "The Password has been reset";
                return View("ListUsers", await GetUserwithRoles(Search, 1));
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<JsonResult> SearchResult(string Search) =>
            await SearchJson(Search);
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return Json($"The user name {username} is already in use.");
            }
            return Json(true);
        }

        #region Private Methods
        async Task<JsonResult> SearchJson(string Search)
        {
            var task = Task.Factory.StartNew<JsonResult>(() =>
            {
                var SearchResult = _userManager.Users.Select(x => x.Name).Where(x => x.ToLower()
                     .Contains(Search.ToLower())).ToList();
                SearchResult.Sort();
                JsonResult result = new JsonResult(SearchResult, new Newtonsoft.Json.JsonSerializerSettings { MaxDepth = 6 });
                return result;
            });
            await task;
            return task.Result;
        }

        List<IdentityError> AddErrorsFromResult(IdentityResult result)
        {
            return result.Errors.ToList();
        }

        async Task<UsersPaging> GetUserwithRoles(string Search, int Page)
        {
            List<UserandRole> Result = await GetUserwithRoles(Search);
            return new UsersPaging
            {
                UserandRoles = Result.Skip((Page - 1) * PageSize).
                Take(PageSize).OrderBy(x => x.User.Name).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Result.Count
                },
                Search = Search
            };
        }

        async Task<List<UserandRole>> GetUserwithRoles(string Search)
        {
            List<UserandRole> Result = new List<UserandRole>();
            List<User> users = new List<User>();
            if (string.IsNullOrEmpty(Search))
            {
                users = _userManager.Users.ToList();
            }
            else
            {
                users = _userManager.Users.Where(x => x.Name.ToLower()
               .Contains(Search.ToLower())).ToList();
            }
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                Result.Add(new UserandRole { User = user, Role = role });
            }
            return Result;
        }

        async Task<EditUser> EditUser(string username, string Search)
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
                Staff = role.LastOrDefault(),
                Search = Search,
                Phone = user.PhoneNumber,
                Qualification = user.Qualification,
                Specialization = user.Specialization
            };
        }

        string GetSpecialization(string Staff, string Specialization)
        {
            string specialies = null;
            if (Staff == "Doctor" || Staff == "Head quarter" || Staff == "Manager")
            {
                specialies = Specialization;
            }
            return specialies;
        }

        async Task AddUserToRole(User user, string Role, string Specialization)
        {
            if (await RoleManager.FindByNameAsync(Role) == null)
            {
                await RoleManager.CreateAsync(new IdentityRole(Role));
            }
            await _userManager.AddToRoleAsync(user, Role);
            if (Role == "Doctor" || Role == "Head quarter" || Role == "Manager")
            {
                if (await RoleManager.FindByNameAsync(Specialization) == null)
                {
                    await RoleManager.CreateAsync(new IdentityRole(Specialization));
                }
                await _userManager.AddToRoleAsync(user, Specialization);
            }
        }
        #endregion
    }
}