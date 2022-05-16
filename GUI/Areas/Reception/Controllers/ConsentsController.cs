using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Reception.Controllers
{
    public class ConsentsController : CommonReception
    {
        #region Constractor
        public ConsentsController(IClinicRepository clinicRepository,
            ICompanyRepository companyRepository,
            IFollowUpRepository followUpRepository,
            IPatientRepository patientRepository,
            IAdmissionsRepository admissionsRepository,
            IReceptionService receptionService,
            IConsentsRepository consentsRepository,
            ILogFileServices logFileServices,
            UserManager<User> userManager) :
            base(clinicRepository, companyRepository, followUpRepository,
                patientRepository, admissionsRepository, receptionService,
                consentsRepository, logFileServices, userManager)
        {
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index() =>
            View(await ConsentsRepository.Reception_Select_GetConsents());
        [HttpPost]
        public async Task<IActionResult> Add(Consent Consent)
        {
            if (ModelState.IsValid)
            {
                var Result = await ReceptionService.AddConsent(Consent);
                if (Result.Success)
                {
                    TempData["success"] = "Consnet has been added successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add new Consent to the system " +
                        $"which name is {Consent.Description} "
                    });
                    var Consents = await ConsentsRepository.
                        Reception_Select_GetConsents();
                    return PartialView("ListConsents", Consents);
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Personid)
        {
            if (string.IsNullOrEmpty(Personid))
            {
                return RedirectToAction("Index", "Patients", null);
            }
            var Consent = await ConsentsRepository.
                Reception_Select_GetConsents_by_ID(Personid);
            return View(Consent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Consent Consent)
        {
            var Result = await ConsentsRepository.
                Reception_Edit_Consent(Consent);
            if (Result.Success)
            {
                TempData["success"] = "Consnet has been Modified successfully";
                var user = await CurrentUser;
                await LogFileServices.AddLogFile(new LogFile
                {
                    UserID = user.Id,
                    Description = $"{user.Name} edit Consent at the system " +
                    $"which new name might become or still the same {Consent.Description} "
                });
                return RedirectToAction("Index");
            }
            TempData["Error"] = Result.ErrorMessage;
            return View(Consent);
        }

    }
}