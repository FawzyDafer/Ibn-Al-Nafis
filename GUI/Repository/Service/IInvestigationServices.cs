using GUI.Areas.Laps.Models;
using GUI.Areas.Medical.Models;
using GUI.Models.Entities;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository.Service
{
    public interface IInvestigationServices
    {
        #region Get Methods
        Task<EditInvestigation> GetSelectedInvestigation(string ID);
        Task<Dictionary<long, InvestigationView>> GetInvestigationView(string Sort);
        Task<Dictionary<long, RayView>> GetRayView(string Sort);
        Task<InvestigationView> EditGetInvestigationView(string PIID);
        Task<RayView> EditGetRayView(string PIID);
        Task<GetPatientInvestigation> RetrievePatientData(long Admission, string Sort);
        Task<GetPatientInvestigation> RetrieveAdmissionData(long Admission, string Sort);
        Task<GetPatientInvestigation> GetInvestigationData(long Admission, string PIID);
        Task<GetPatientInvestigation> GetRayData(long Admission, string PIID);
        Task<Patient> GetPatientbyPIID(string PIID);
        #endregion

        #region Operation Methods
        Task<StoredResult> AddNewLap(Investigation Investigation);

        Task<StoredResult> RequestInvestigation(PatientInvestigation PatientInvestigation);

        Task<StoredResult> AddPatientRay(EditPatientRay EditRay);
        #endregion
    }
}
