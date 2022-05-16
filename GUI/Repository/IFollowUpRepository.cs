using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IFollowUpRepository
    {
        #region GetMethods
        Task<Dictionary<string, PatientsFollowUps>>
            Reception_Select_Patient_by_FllowUpdate();
        Task<Dictionary<string, PatientsFollowUps>>
            Doctor_Select_FoolowUp_by_Admission(long AdmissionID);
        #endregion

        #region Operation
        Task<StoredResult> Doctor_Add_NewFollowUp(FllowUp FllowUp);
        Task<StoredResult> Doctor_Edit_NewFollowUp(FllowUp FllowUp);
        Task<StoredResult> Reception_Edit_FollowUp(long Admission);
        #endregion
    }
}
