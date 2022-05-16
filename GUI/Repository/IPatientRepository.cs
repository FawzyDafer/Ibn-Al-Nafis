using GUI.Models.Entities;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IPatientRepository
    {
        #region GetMethods
        Task<Dictionary<string, Patient>> Reception_Select_Patient_by_Name(string Name);
        Task<Dictionary<string, Patient>> Reception_Select_Patient_by_Phone(string Phone);
        Task<Patient> Reception_Select_Patient_by_SSN(string SSN);
        #endregion

        #region Operation
        Task<StoredResult> Reception_Add_Patient(Patient Patient);
        Task<StoredResult> Reception_Edit_Patient(Patient Patient);
        #endregion

    }
}
