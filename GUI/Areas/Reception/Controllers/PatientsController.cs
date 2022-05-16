using GUI.Areas.Reception.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GUI.Areas.Reception.Controllers
{
    public class PatientsController : CommonReception
    {
        #region Constractor
        public PatientsController(IClinicRepository clinicRepository,
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
            View(new PatientRegestiration()
            {
                Patient = new Patient { Address = "مصر" },
                Clinics = await ClinicRepository.Reception_Select_Clinic_by_Day()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PatientRegestiration Reception)
        {
            if (ModelState.IsValid)
            {
                var result = await ReceptionService.RegisteratePatient
                    (Reception.Patient, Reception.PatientClinic);
                if (result.Success)
                {
                    TempData["success"] = "Patient has been added successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} add new patient to the " +
                        $"system with ssn {Reception.Patient.SSN} and name {Reception.Patient.Name}"
                    });
                    int age = new DateTime(DateTime.Now.
                        Subtract(Reception.Patient.DateOfBirth).Ticks).Year - 1;
                    if (age < 18)
                    {
                        return RedirectToAction("AddRelative",
                            new { ssn = Reception.Patient.SSN, Admission = result.LongID });
                    }
                    return RedirectToAction("Sheat",
                        new
                        {
                            ssn = Reception.Patient.SSN,
                            Admission = result.LongID
                        });
                }
                ViewData["erro"] = result.ErrorMessage;
            }
            Reception.Clinics =
                await ClinicRepository.Reception_Select_Clinic_by_Day();
            return View(Reception);
        }
        [HttpGet]
        public async Task<IActionResult>
            Edit(string ssn, string Clinic, long AdmissionID = -1)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return RedirectToAction("Index");
            }
            return View(new PatientRegestiration
            {
                Patient = await PatientRepository.Reception_Select_Patient_by_SSN(ssn),
                PatientClinic = new Admission() { Clinic = Clinic, Counter = AdmissionID },
                Clinics = await ClinicRepository.Reception_Select_Clinic_by_Day()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PatientRegestiration Reception)
        {
            if (ModelState.IsValid)
            {
                var result = await ReceptionService.EditRegisteratePatient
                    (Reception.Patient, Reception.PatientClinic);
                if (result.Success)
                {
                    TempData["success"] = "Patient has been edited successfully";
                    var user = await CurrentUser;
                    await LogFileServices.AddLogFile(new LogFile
                    {
                        UserID = user.Id,
                        Description = $"{user.Name} edite patient Data " +
                         $"whose {Reception.Patient.SSN} and name {Reception.Patient.Name}"
                    });
                    if (Reception.PatientClinic.Counter != -1)
                    {
                        await FollowUpRepository.Reception_Edit_FollowUp
                            (Reception.PatientClinic.Counter);
                    }
                    return RedirectToAction("Sheat", new
                    {
                        ssn = Reception.Patient.SSN,
                        Admission = result.LongID
                    });
                }
                ViewData["erro"] = result.ErrorMessage;
            }
            Reception.Clinics =
                await ClinicRepository.Reception_Select_Clinic_by_Day();
            return View(Reception);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string Search, int page = 1) =>
            View(await SearchPatient(Search, page));
        [HttpGet]
        public async Task<IActionResult> Sheat(long Admission)
        {
            if (Admission == 0)
            {
                return RedirectToAction("Index");
            }
            var PatientClinic = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(Admission);
            return View(new PatientRegestiration
            {
                Patient = await PatientRepository.Reception_Select_Patient_by_SSN(PatientClinic.PatientsSSN),
                PatientClinic = PatientClinic,
                Clinics = await ClinicRepository.Reception_Select_Clinic_by_Day()
            });
        }

        public async Task<JsonResult> SearchResult(string Search) =>
            await SearchJson(Search);
        [HttpGet]
        public async Task<IActionResult> FollowUpRecord(string Search, int Page = 1) =>
            View(await SearchPatientFollowUp(Search, Page));
        [HttpGet]
        public async Task<IActionResult> RecentAddPatient() =>
            View(await SearchPatientRecent());
    }
}