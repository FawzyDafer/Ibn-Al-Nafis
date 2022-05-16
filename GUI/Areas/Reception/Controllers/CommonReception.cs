using GUI.Areas.Reception.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Reception.Controllers
{
    [Authorize(Roles = "Receptionist, Manager")]
    [AutoValidateAntiforgeryToken]
    public class CommonReception : Controller
    {
        #region Vaiables
        readonly protected IClinicRepository ClinicRepository;
        readonly protected ICompanyRepository CompanyRepository;
        readonly protected IAdmissionsRepository AdmissionsRepository;
        readonly protected IFollowUpRepository FollowUpRepository;
        readonly protected IPatientRepository PatientRepository;
        readonly protected IReceptionService ReceptionService;
        readonly protected IConsentsRepository ConsentsRepository;
        readonly protected ILogFileServices LogFileServices;
        readonly protected UserManager<User> _userManager;
        protected Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        readonly protected int PageSize = 30;
        #endregion

        #region Constructors
        public CommonReception(IClinicRepository clinicRepository,
            ICompanyRepository companyRepository,
            IFollowUpRepository followUpRepository,
            IPatientRepository patientRepository,
            IAdmissionsRepository admissionsRepository,
            IReceptionService receptionService,
            IConsentsRepository consentsRepository,
            ILogFileServices logFileServices,
            UserManager<User> userManager)
        {
            ClinicRepository = clinicRepository;
            CompanyRepository = companyRepository;
            FollowUpRepository = followUpRepository;
            PatientRepository = patientRepository;
            ReceptionService = receptionService;
            AdmissionsRepository = admissionsRepository;
            ConsentsRepository = consentsRepository;
            LogFileServices = logFileServices;
            _userManager = userManager;
        }
        #endregion

        #region Protected Methods
        protected async Task<JsonResult> SearchJson(string Search)
        {
            var SearchResult = await PatientRepository.Reception_Select_Patient_by_Name(Search);
            var ListResult = SearchResult.Values.Select(x => x.Name).Take(6).ToList();
            JsonResult result = new JsonResult(ListResult);
            return result;
        }

        protected async Task<PatientSearch> SearchPatient(string ssn, int Page)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return new PatientSearch
                {
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = Page,
                        ItemsPerPage = PageSize,
                        TotalItems = 0
                    }
                };
            }
            var patient = await ReceptionService.SearchForPatient(ssn);
            return new PatientSearch
            {
                Patients = patient.Skip((Page - 1) * PageSize).
                Take(PageSize).OrderBy(x => x.Value.Name).ToDictionary(x => x.Key, x => x.Value),
                Search = ssn,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = patient.Count
                }
            };
        }

        protected async Task<PatientSearch> SearchPatientFollowUp(string ssn, int Page)
        {
            var FollowUps = (string.IsNullOrEmpty(ssn)) ?
                await FollowUpRepository.Reception_Select_Patient_by_FllowUpdate() :
                await ReceptionService.SelectFollowUpbyPatient(ssn);
            return new PatientSearch
            {
                PatientsFollowUps = FollowUps.Skip((Page - 1) * PageSize).
                Take(PageSize).OrderBy(x => x.Value.Name).ToDictionary(x => x.Key, x => x.Value),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = FollowUps.Count
                }
            };
        }

        protected async Task<PatientSearch> SearchPatientRecent() =>
            new PatientSearch
            {
                PatientRegestiration = await ReceptionService.
                SelectRegistratedPatient()
            };

        protected async Task<Patient> GetPatientAsync(long Admission)
        {
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(Admission);
            return await PatientRepository.
                Reception_Select_Patient_by_SSN(admission.PatientsSSN);
        }

        #endregion
    }
}