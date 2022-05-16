using GUI.Models.Identity;
using GUI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Medical.Controllers
{
    [Authorize(Roles = "Doctor, Manager, Head quarter")]
    [AutoValidateAntiforgeryToken]
    public class DepartmentController : Controller
    {
        #region Private Variables
        readonly IClinicRepository ClinicRepository;
        readonly IInvestigationRepository InvestigationRepository;
        readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> RoleManager;
        Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Controller
        public DepartmentController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IClinicRepository clinicRepository,
            IInvestigationRepository investigationRepository)
        {
            RoleManager = roleManager;
            _userManager = userManager;
            ClinicRepository = clinicRepository;
            InvestigationRepository = investigationRepository;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var user = await CurrentUser;
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == "Manager")
            {
                return View(await ClinicRepository.Admin_Select_Clinics());
            }
            if (!string.IsNullOrEmpty(user.Clinic))
            {
                return Redirect($"/Medical/{user.Clinic}/Index");
            }
            return RedirectToAction("Index", "Home", null);
        }

        public async Task<FileStreamResult> ViewImage(string uname)
        {
            var File = await InvestigationRepository.
                Investigation_Select_InvesigationFiles_by_ID(uname);

            MemoryStream ms = new MemoryStream(File.FileData);

            return new FileStreamResult(ms, File.FileExtention);
        }

    }
}