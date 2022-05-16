using GUI.Areas.Reception.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.Reception.Controllers
{
    public class RelativeController : CommonReception
    {
        #region Constractor
        public RelativeController(IClinicRepository clinicRepository,
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
        public async Task<IActionResult> Index(string ssn, long Admission)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return RedirectToAction("Index", "Patients", null);
            }
            return View(new GetRelative
            {
                Companies = await ReceptionService.SelectPatientCompany(ssn),
                SSN = ssn,
                AdmissionID = Admission
            });
        }
        [HttpGet]
        public IActionResult Add(string ssn, long Admission)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return RedirectToAction("Index", "Patients", null);
            }
            return View(new CreatePatientRelative()
            {
                Relative = new Person { Address = "مصر" },
                PatientRelative = new PatientRelative { PatientSSN = ssn, AdmissionID = Admission }
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreatePatientRelative CreatePatinetRelative)
        {
            if (ModelState.IsValid)
            {
                var Result = await ReceptionService.AddCompanyToPatient(
                     CreatePatinetRelative.PatientRelative,
                     CreatePatinetRelative.Relative);
                if (Result.Success)
                {
                    TempData["success"] = "The Relative has been added successfuly";
                    var user = await CurrentUser;
                    var patient = await PatientRepository.
                        Reception_Select_Patient_by_SSN
                        (CreatePatinetRelative.PatientRelative.PatientSSN);
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add new patient relative to patient" +
                        $" whose ssn {patient.SSN} and his name is {patient.Name}" +
                        $"and Relative ssn {CreatePatinetRelative.Relative.SSN} " +
                        $"and name {CreatePatinetRelative.Relative.Name}"
                    });
                    return RedirectToAction("Index", new
                    {
                        ssn = CreatePatinetRelative.PatientRelative.PatientSSN,
                        Admission = CreatePatinetRelative.PatientRelative.AdmissionID
                    });
                }
                ViewData["erro"] = Result.ErrorMessage;
            }
            return View(CreatePatinetRelative);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string ssn, long Admission, string Personid)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return RedirectToAction("Index", "Patients", null);
            }
            var person = await ReceptionService.
                SelectPatientRelativebyID(ssn, Personid);
            person.PatientRelative.AdmissionID = Admission;
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreatePatientRelative CreatePatinetRelative)
        {
            if (ModelState.IsValid)
            {
                var Result = await CompanyRepository.
                    Reception_Edit_Company(CreatePatinetRelative.Relative);
                if (Result.Success)
                {
                    await CompanyRepository.
                        Reception_Add_PatientCompany
                        (CreatePatinetRelative.PatientRelative);
                    TempData["success"] = "Data has been modified successfuly";
                    var user = await CurrentUser;
                    var patient = await PatientRepository.
                        Reception_Select_Patient_by_SSN
                        (CreatePatinetRelative.PatientRelative.PatientSSN);
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add new patient relative to patient" +
                        $" whose ssn {patient.SSN} and his name is {patient.Name}" +
                        $"and Relative ssn {CreatePatinetRelative.Relative.SSN} " +
                        $"and name {CreatePatinetRelative.Relative.Name}"
                    });
                    return RedirectToAction("Index", new
                    {
                        ssn = CreatePatinetRelative.PatientRelative.PatientSSN,
                        Admission = CreatePatinetRelative.PatientRelative.AdmissionID
                    });
                }
                ViewData["erro"] = Result.ErrorMessage;
            }
            return View(CreatePatinetRelative);
        }

    }
}