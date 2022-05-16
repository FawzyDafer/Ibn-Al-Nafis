using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace GUI.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Manager")]
    public class SliderController : Controller
    {
        #region Private Variables
        readonly IImageService ImageService;
        readonly UserManager<User> _userManager;
        readonly ILogFileServices LogFileServices;
        Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constractor
        public SliderController(IImageService imageService,
            ILogFileServices logFileServices,
            UserManager<User> userManager)
        {
            ImageService = imageService;
            LogFileServices = logFileServices;
            _userManager = userManager;
        }
        #endregion

        public async Task<IActionResult> Index() =>
            View(await ImageService.GetImageByCategory("Slider"));
        [HttpGet]
        public async Task<IActionResult> Edit(string uname) =>
            View(await ImageService.GetImageById(uname));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Images images)
        {
            if (ModelState.IsValid)
            {
                images.Category = "Slider";
                if (string.IsNullOrEmpty(images.Title))
                    images.Title = " ";
                var Result = await ImageService.EditImage(images);
                if (Result.Success)
                {
                    TempData["Sucssess"] = "The Image has been modified successfully.";
                    var existeduser = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = existeduser.Id,
                        Description = $"{existeduser.Name} Edit Slider Image"
                    });
                    return RedirectToAction("Index");
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return View(images);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<FileStreamResult> ViewImage(string uname)
        {
            var image = await ImageService.GetImageById(uname);

            MemoryStream ms = new MemoryStream(image.ImageData);
            return new FileStreamResult(ms, image.ImageExtention);
        }
    }
}