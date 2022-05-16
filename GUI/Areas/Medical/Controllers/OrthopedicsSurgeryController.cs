using GUI.Areas.Medical.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Medical.Controllers
{
    [Authorize(Roles = "Orthopedics Surgery, Manager, Head quarter")]
    public class OrthopedicsSurgeryController : DoctorController
    {
        #region Vaiables
        readonly string ClinicTitle = "Orthopedics Surgery";
        readonly string controller;
        #endregion

        #region Constructors
        public OrthopedicsSurgeryController(IClinicRepository clinicRepository,
            ILogFileServices logFileServices, IDoctorService doctorService,
            IReceptionService receptionService, IAdmissionsRepository admissionsRepository,
            IInvestigationRepository investigationRepository, IInvestigationServices investigationServices,
            UserManager<User> userManager) :
            base(clinicRepository, logFileServices, doctorService, receptionService, admissionsRepository, investigationRepository,
                investigationServices, userManager)
        {
            controller = ClinicTitle.Replace(" ", string.Empty);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (!ValidateAdmission(Admission))
            {
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await ReceptionService.DoctorSearchPatientRecent(ClinicTitle));
        }
        [HttpGet]
        [Authorize(Roles = "Manager, Head quarter")]
        public async Task<IActionResult> Settings(long? Admission)
        {
            var Day = await DoctorService.GetClincWorkingDays(ClinicTitle);
            Day.Admission = Admission;
            ViewData["clinic"] = ClinicTitle;
            return View(Day);
        }
        [HttpPost]
        [Authorize(Roles = "Manager, Head quarter")]
        public async Task<IActionResult> Settings(ClincWorkingDays ClincWorkingDays)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                var Result = await DoctorService.SetClincWorkingDays(ClincWorkingDays);
                if (Result.Success)
                {
                    var existeduser = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = existeduser.Id,
                        Description = $"{existeduser.Name} Change {ClinicTitle} working days"
                    });
                    TempData["Success"] = $"{ClinicTitle} clinic working days has been modified successfully";
                    return RedirectToAction("Settings");
                }
                else
                {
                    TempData["Error"] = Result.ErrorMessage;
                }
            }
            return View(ClincWorkingDays);
        }
        [HttpGet]
        public async Task<IActionResult> History(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            var existeduser = await CurrentUser;
            var patient = await ReceptionService.GetPatientAsync(Admission.Value);
            await LogFileServices.AddLogFile(new LogFile
            {
                UserID = existeduser.Id,
                Description = $"{existeduser.Name} begin to examine the patient whose name is" +
                $"{patient.Name} and ssn in {patient.SSN}"
            });
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(Admission.Value);
            await AdmissionsRepository.Doctor_Edit_Admission(admission);
            return View(await DoctorService.GetAdmissionHistory(Admission.Value));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> History(AdmissionHistory admissionHistory)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                admissionHistory.DoctorID = (await CurrentUser).Id;
                await DoctorService.AddHistory(admissionHistory);
                return RedirectToAction("Examination", controller, new { admissionHistory.Admission });
            }
            return View(admissionHistory);
        }

        #region CoefficientAnalytics

        [HttpGet]
        public async Task<IActionResult> RequestaCoefficientAnalytics(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(new RequestPatientInvestigation()
            {
                Investigation = await InvestigationRepository.
                   Investigation_Select_InvesigationCategory_by_Sort(LapSort),
                AdmessionID = Admission.Value
            });
        }
        [HttpPost]
        public async Task<IActionResult> RequestaCoefficientAnalytics
            (RequestPatientInvestigation RequestPatientInvestigation)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                var Result = await InsertMultible(RequestPatientInvestigation);
                if (Result.Success)
                {
                    return Ok();
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> RetrieveCoefficientAnalytics
            (long? Admission, int page = 1, string Search = null)
        {
            ViewData["clinic"] = ClinicTitle;
            Admission = (string.IsNullOrEmpty(Search)) ? Admission : long.Parse(Search);
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await GetPatientInvestigation(Admission.Value, page, LapSort));
        }
        [HttpGet]
        public async Task<IActionResult>
            RecentOrderCoefficientAnalytics(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await InvestigationServices.
                RetrieveAdmissionData(Admission.Value, LapSort));
        }
        [HttpGet]
        public async Task<IActionResult>
            GetCoefficientAnalyticsDetatils(string uname, long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await InvestigationServices.GetInvestigationData(Admission.Value, uname));
        }

        #endregion

        #region Rays
        [HttpGet]
        public async Task<IActionResult> RequestaRay(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(new RequestPatientInvestigation()
            {
                Investigation = await InvestigationRepository.
                   Investigation_Select_InvesigationCategory_by_Sort(RaySort),
                AdmessionID = Admission.Value
            });
        }
        [HttpPost]
        public async Task<IActionResult> RequestaRay
            (RequestPatientInvestigation RequestPatientInvestigation)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                var Result = await InsertMultible(RequestPatientInvestigation);
                if (Result.Success)
                {
                    return Ok();
                }
                TempData["Error"] = Result.ErrorMessage;
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> RetrieveRays
            (long? Admission, int page = 1, string Search = null)
        {
            ViewData["clinic"] = ClinicTitle;
            Admission = (string.IsNullOrEmpty(Search)) ? Admission : long.Parse(Search);
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await GetPatientInvestigation(Admission.Value, page, RaySort));
        }
        [HttpGet]
        public async Task<IActionResult>
            RecentOrderRays(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await InvestigationServices.
                RetrieveAdmissionData(Admission.Value, RaySort));
        }
        [HttpGet]
        public async Task<IActionResult>
            GetRayDetatils(string uname, long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await InvestigationServices.GetRayData(Admission.Value, uname));
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Examination(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            return View(await DoctorService.GetDiagnose(Admission.Value, ClinicTitle));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Examination(Digonose digonose)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                digonose.DoctorID = (await CurrentUser).Id;
                await DoctorService.AddDiagnose(digonose);
                TempData["Sucssess"] = "Diagnose added successfully";
            }
            return View(digonose);
        }
        [HttpGet]
        public async Task<IActionResult> AddFollowUp(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            if (await DoctorService.ValidateExamination(Admission.Value))
            {
                TempData["error"] = "Please Examination patient Examinations First";
                return RedirectToAction("Examination", controller, new { Admission });
            }
            return View(await DoctorService.GetFllowUp(Admission.Value));
        }
        [HttpPost]
        public async Task<IActionResult> AddFollowUp(FllowUp FllowUp)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                var user = await CurrentUser;
                FllowUp.DoctorID = user.Id;
                var Result = await DoctorService.AddFollowUp(FllowUp);
                if (Result.Success)
                {
                    TempData["Sucssess"] = "Fllow Up has been added successfuly";
                }
                else
                {
                    TempData["Error"] = Result.ErrorMessage;
                }
            }
            return View(FllowUp);
        }
        [HttpGet]
        public async Task<IActionResult> Discharge(long? Admission)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ValidateAdmission(Admission))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("Index", controller, null);
            }
            if (await DoctorService.ValidateHistory(Admission.Value))
            {
                TempData["error"] = "Please add patient history First";
                return RedirectToAction("History", controller, new { Admission });
            }
            if (await DoctorService.ValidateExamination(Admission.Value))
            {
                TempData["error"] = "Please Examination patient Examinations First";
                return RedirectToAction("Examination", controller, new { Admission });
            }
            return View(new Discharge() { AdmissionID = Admission.Value });
        }
        [HttpPost]
        public async Task<IActionResult> Discharge(Discharge discharge)
        {
            ViewData["clinic"] = ClinicTitle;
            if (ModelState.IsValid)
            {
                ViewData["clinic"] = ClinicTitle;
                discharge.DoctorID = (await CurrentUser).Id;
                var result = await DoctorService.Discharge(discharge);
                if (result.Success)
                {
                    return RedirectToAction("Index", controller, null);
                }
                TempData["error"] = result.ErrorMessage;
            }
            return View(discharge);
        }
    }
}