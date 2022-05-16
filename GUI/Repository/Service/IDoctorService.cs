using GUI.Areas.Medical.Models;
using GUI.Models.Entities;
using M3Y.Entities;
using System.Threading.Tasks;

namespace GUI.Repository.Service
{
    public interface IDoctorService
    {
        #region Get Methods
        Task<ClincWorkingDays> GetClincWorkingDays(string ClinicTitle);
        Task<AdmissionHistory> GetAdmissionHistory(long admission);
        Task<Digonose> GetDiagnose(long admission, string Clinic);
        Task<FllowUp> GetFllowUp(long admission);
        Task<bool> ValidateHistory(long Admission);
        Task<bool> ValidateExamination(long Admission);
        #endregion

        #region Operation Methods
        Task<StoredResult> AddFollowUp(FllowUp FllowUp);
        Task<StoredResult> SetClincWorkingDays(ClincWorkingDays ClincWorkingDays);
        Task AddHistory(AdmissionHistory admissionHistory);
        Task AddDiagnose(Digonose Digonose);
        Task<StoredResult> Discharge(Discharge discharge);
        #endregion
    }
}
