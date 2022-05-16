using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Models.Views;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace GUI.Areas.Reception.Controllers
{
    public class PatientConsentsController : CommonReception
    {
        #region Constractor
        public PatientConsentsController(IClinicRepository clinicRepository,
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
        public async Task<IActionResult> Index(long Admission) =>
            View(new GetPatientConsent
            {
                PatientsConsents = await ConsentsRepository.
                                Reception_Select_Consents_by_Admission(Admission),
                Admission = Admission
            });
        [HttpGet]
        public async Task<IActionResult> Add(long Admission)
            => View(new PatientConsent()
            {
                AdmissionID = Admission,
                Consents = await ConsentsRepository.Reception_Select_GetConsents()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PatientConsent PatientConsent)
        {
            if (ModelState.IsValid)
            {
                var Result = await ReceptionService.
                    AddConsenttoPatient(PatientConsent);
                if (Result.Success)
                {
                    TempData["success"] = "Consent has been added successfully";
                    var user = await CurrentUser;
                    var concent = await ConsentsRepository.
                        Reception_Select_GetConsents_by_ID(PatientConsent.ConID);
                    var patient = await GetPatientAsync(PatientConsent.AdmissionID);
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add Consent to patient " +
                        $" whose ssn {patient.SSN} and his name is {patient.Name} " +
                        $"and concent is {concent.Description} "
                    });
                    return RedirectToAction("Index",
                        new { Admission = PatientConsent.AdmissionID });
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            PatientConsent.Consents =
                await ConsentsRepository.Reception_Select_GetConsents();
            return View(PatientConsent);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long Admission, string Personid)
        {
            if (string.IsNullOrEmpty(Personid))
            {
                return RedirectToAction("Index", "Patients", null);
            }
            var person = await ConsentsRepository.
                Reception_Select_Consents_by_ID(Personid);
            return View(new EPatientConsent
            {
                PCID = Personid,
                AdmissionID = person.AdmissionID,
                ConID = person.ConID,
                ImageData = person.ImageData,
                ImageExtention = person.ImageExtention,
                Consents = await ConsentsRepository.Reception_Select_GetConsents()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EPatientConsent PatientConsent)
        {
            if (ModelState.IsValid)
            {
                var Result = await ReceptionService.
                    EditConsenttoPatient(new
                    PatientConsent
                    {
                        PCID = PatientConsent.PCID,
                        AdmissionID = PatientConsent.AdmissionID,
                        ConID = PatientConsent.ConID,
                        ImageData = PatientConsent.ImageData,
                        ImageExtention = PatientConsent.ImageExtention,
                        Image = PatientConsent.Image
                    });
                if (Result.Success)
                {
                    TempData["success"] = "Consent has been modified successfuly";
                    var user = await CurrentUser;
                    var concent = await ConsentsRepository.
                        Reception_Select_GetConsents_by_ID(PatientConsent.ConID);
                    var patient = await GetPatientAsync(PatientConsent.AdmissionID);
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add Consent to patient " +
                        $" whose ssn {patient.SSN} and his name is {patient.Name} " +
                        $"and concent is {concent.Description} "
                    });
                    return RedirectToAction("Index",
                        new { Admission = PatientConsent.AdmissionID });
                }
                ViewData["erro"] = Result.ErrorMessage;
            }
            PatientConsent.Consents = await ConsentsRepository.Reception_Select_GetConsents();
            return View(PatientConsent);
        }
        [HttpGet]
        public async Task<FileStreamResult> ViewImage(string Personid)
        {
            var person = await ConsentsRepository.
                Reception_Select_Consents_by_ID(Personid);
            MemoryStream ms = new MemoryStream(person.ImageData);
            return new FileStreamResult(ms, person.ImageExtention);
        }
    }
}