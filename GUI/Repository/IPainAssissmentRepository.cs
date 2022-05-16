using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IPainAssissmentRepository
    {
        #region GetMethods
        Task<PatientsPains> History_Select_Pain_By_AdmissionID(long AdmissionID);
        Task<Dictionary<string, PatientsPains>> History_Select_Pain_By_PatientsSSN(string PatientsSSN);
        Task<PatientsPains> History_Select_Pain_By_PainID(string PainID);
        #endregion

        #region Operation Methods
        Task<StoredResult> History_Add_Assissment(PainAssissment Pain);
        Task<StoredResult> History_Edit_Assissment(PainAssissment Pain);
        #endregion
    }
}
