using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IConsentsRepository
    {
        #region GetMethods
        Task<Dictionary<long, PatientsConsents>> Reception_Select_Consents();
        Task<Dictionary<long, PatientsConsents>> Reception_Select_Consents_by_Admission(long AdmissionID);
        Task<Dictionary<long, PatientsConsents>> Reception_Select_Consents_by_Consent(string Consent);
        Task<Dictionary<long, PatientsConsents>> Reception_Select_Consents_by_Patient(string SSN);
        Task<PatientsConsents> Reception_Select_Consents_by_ID(string PCID);
        Task<Dictionary<string, Consent>> Reception_Select_GetConsents();
        Task<Consent> Reception_Select_GetConsents_by_ID(string ConID);
        #endregion

        #region OperationMethods
        Task<StoredResult> Reception_Add_Consent(Consent Consent);
        Task<StoredResult> Reception_Edit_Consent(Consent Consent);
        Task<StoredResult> Reception_Add_PatientConsent(PatientConsent PatientConsent);
        Task<StoredResult> Reception_Edit_PatientConsent(PatientConsent PatientConsent);
        #endregion

    }
}
