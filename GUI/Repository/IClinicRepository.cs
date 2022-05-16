using GUI.Models.Entities;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IClinicRepository
    {
        #region GetMethods
        Task<Dictionary<string, Clinic>> Reception_Select_Clinic_by_Day();
        Task<Dictionary<string, Clinic>> Admin_Select_Clinics();
        Task<List<ClinicsDay>> Admin_Select_ClinicDays();
        Task<List<ClinicsDay>> Admin_Select_ClinicDays_by_Clinic(string Clinic);
        #endregion

        #region Operation
        Task<StoredResult> Admin_Add_ClinicDays(ClinicsDay ClinicsDay);
        Task<StoredResult> Admin_Delete_ClinicDays_by_Clinic(string Clinic);
        #endregion
    }
}
