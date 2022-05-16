using GUI.Areas.Medical.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using M3Y.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Medical.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class DoctorController : Controller
    {
        #region Private Variables
        readonly protected IClinicRepository ClinicRepository;
        readonly protected ICompanyRepository CompanyRepository;
        readonly protected IAdmissionsRepository AdmissionsRepository;
        readonly protected IDoctorService DoctorService;
        readonly protected IReceptionService ReceptionService;
        readonly protected IInvestigationRepository InvestigationRepository;
        readonly protected IInvestigationServices InvestigationServices;
        readonly protected ILogFileServices LogFileServices;
        readonly protected UserManager<User> _userManager;
        readonly protected int PageSize = 30;
        protected Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        readonly protected string LapSort = "Lap";
        readonly protected string RaySort = "Ray";
        #endregion

        #region Constructors
        public DoctorController(IClinicRepository clinicRepository,
            ILogFileServices logFileServices,
            IDoctorService doctorService,
            IReceptionService receptionService,
            IAdmissionsRepository admissionsRepository,
            IInvestigationRepository investigationRepository,
            IInvestigationServices investigationServices,
            UserManager<User> userManager)
        {
            ClinicRepository = clinicRepository;
            LogFileServices = logFileServices;
            AdmissionsRepository = admissionsRepository;
            DoctorService = doctorService;
            ReceptionService = receptionService;
            InvestigationRepository = investigationRepository;
            InvestigationServices = investigationServices;
            _userManager = userManager;
        }
        #endregion

        #region Protected Methods
        protected bool ValidateAdmission(long? Admission)
        {
            if (Admission.HasValue)
            {
                if (Admission <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        protected async Task<StoredResult> InsertMultible(RequestPatientInvestigation investigation)
        {
            StoredResult Result = new StoredResult();
            var Doctor = await CurrentUser;
            foreach (var item in investigation.Types)
            {
                Result = await InvestigationServices.
                   RequestInvestigation(new PatientInvestigation()
                   {
                       AdmessionID = investigation.AdmessionID,
                       Type = item,
                       DoctorID = Doctor.Id,
                       Finish = false,
                       Note = (string.IsNullOrEmpty(investigation.Note)) ? "-" : investigation.Note
                   });
            }
            Result.Success = true;
            return Result;
        }

        protected async Task<GetPatientInvestigation> GetPatientInvestigation(long Admission, int Page, string Sort)
        {
            var investigation = await InvestigationServices.
                RetrievePatientData(Admission, Sort);
            return new GetPatientInvestigation
            {
                Investigations = investigation.Investigations.Skip((Page - 1) * PageSize).
                Take(PageSize).ToDictionary(x => x.Key, x => x.Value),
                Admission = investigation.Admission,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = investigation.Investigations.Count
                }
            };
        }
        #endregion
    }
}