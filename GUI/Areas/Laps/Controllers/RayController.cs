using GUI.Areas.Laps.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Laps.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Ray Technical, Manager")]
    public class RayController : LapCommon
    {
        #region Vaiables
        readonly string Sort = "Ray";
        #endregion

        #region Constructors
        public RayController(
            IInvestigationRepository investigationRepository,
            IInvestigationServices investigationServices,
            ILogFileServices logFileServices, IPatientRepository
            patientRepository, UserManager<User> userManager) :
            base(investigationRepository, investigationServices,
                logFileServices, patientRepository, userManager)
        {
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(int Page = 1) =>
            View(await SearchRay(Page, Sort));
        [HttpGet]
        public async Task<IActionResult> AddRayData(string uname) =>
            View(await InvestigationServices.EditGetRayView(uname));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRayData(RayView rayView)
        {
            if (ModelState.IsValid)
            {
                var user = await CurrentUser;
                rayView.EditRay.StaffID = user.Id;
                var Result = await InvestigationServices.
                    AddPatientRay(rayView.EditRay);
                if (Result.Success)
                {
                    TempData["success"] = "Patient investigation Has been Added Successfully";
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add the patient investigation whose name is" +
                        $"{rayView.Patient.Name} and his ssn is {rayView.Patient.SSN}" +
                        $"and the result of the investigation is {rayView.EditRay.Comment}"
                    });
                    return RedirectToAction("Index");
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return View(rayView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRay(Investigation investigation)
        {
            if (ModelState.IsValid)
            {
                investigation.Sort = Sort;
                var Result = await InvestigationServices.AddNewLap(investigation);
                if (Result.Success)
                {
                    TempData["success"] = "Ray Has been Added Successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add new investigation and it is {investigation.Type}" +
                        $"and this investigation under the category of {investigation.CategoryID}"
                    });
                    return Ok();
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetRays(string Search, int Page = 1) =>
            View(await SearchPatient(Search, Page, Sort));
        [HttpGet]
        public async Task<IActionResult> EditRay(string uname)
        {
            var investigation = await InvestigationServices.GetSelectedInvestigation(uname);
            return View(investigation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRay(EditInvestigation EditInvestigation)
        {
            if (ModelState.IsValid)
            {
                EditInvestigation.Investigation.Sort = Sort;
                var Result = await InvestigationRepository.
                    Investigation_Edit_Invesigation(EditInvestigation.Investigation);
                if (Result.Success)
                {
                    TempData["success"] = "Ray Has been Modified Successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} edit new investigation and it is " +
                        $"{EditInvestigation.Investigation.Type} and this investigation under " +
                        $"the category of {EditInvestigation.Investigation.CategoryID}"
                    });
                    return RedirectToAction("GetRays");
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return View(EditInvestigation);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories() =>
            View(await InvestigationRepository.Investigation_Select_Categories(Sort));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(InvestigationCategory InvestigationCategory)
        {
            if (ModelState.IsValid)
            {
                var Result = await InvestigationRepository.
                                                Investigation_Edit_Category(InvestigationCategory);
                if (Result.Success)
                {
                    TempData["success"] = "Category Has been Modified Successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} edit the category name might stay the same" +
                        $" which name is {InvestigationCategory.Category}"
                    });
                    var Category = await InvestigationRepository.
                        Investigation_Select_Categories(Sort);
                    return View("ListCategories", Category);
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return BadRequest();
        }
        public async Task<JsonResult> SearchCatResult(string Search) =>
            await SearchCatJson(Search, Sort);
        public async Task<JsonResult> SearchResult(string Search) =>
            await SearchJson(Search, Sort);
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetSample(bool Checked, string PIID)
        {
            var Result = await InvestigationRepository.
                Investigation_Get_Sample(new PatientInvestigation()
                {
                    PIID = PIID,
                    Finish = Checked
                });
            if (Result.Success)
            {
                var user = await CurrentUser;
                var patient = await InvestigationServices.GetPatientbyPIID(PIID);
                string action = (Checked) ? "take" : "not take";
                await LogFileServices.AddLogFile(new LogFile
                {
                    UserID = user.Id,
                    Description = $"{user.Name} { action} a ray sample from the patient to analyse it" +
                    $" to a patient whose name is {patient.Name} and ssn is {patient.SSN}"
                });
                return Ok();
            }
            return BadRequest();
        }
    }
}