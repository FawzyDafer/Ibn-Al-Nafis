using GUI.Areas.Reception.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GUI.Repository.Service
{
    public interface IReceptionService
    {
        #region GetMethods
        Task<Dictionary<string, Patient>> SearchForPatient(string patient);
        Task<Dictionary<string, CreatePatientRelative>>
            SelectPatientCompany(string patientID);
        Task<CreatePatientRelative>
            SelectPatientRelativebyID(string patientID, string personID);
        Task<Dictionary<string, PatientsFollowUps>>
            SelectFollowUpbyPatient(string Search);
        Task<Dictionary<long, PatientRegestiration>> SelectRegistratedPatient();
        Task<PatientSearch> DoctorSearchPatientRecent(string ClinicTitle);
        Task<Patient> GetPatientAsync(long Admission);
        #endregion

        #region OperationMethods
        Task<StoredResult> AddCompanyToPatient(
         PatientRelative PatientRelative,
         Person Company);
        Task<StoredResult> RegisteratePatient(
            Patient Patient, Admission Admission);
        Task<StoredResult> EditRegisteratePatient(
            Patient Patient, Admission Admission);
        Task<StoredResult> AddConsent(Consent Consent);
        Task<StoredResult> AddConsenttoPatient(PatientConsent Consent);
        Task<StoredResult> EditConsenttoPatient(PatientConsent Consent);
        #endregion
    }
}
