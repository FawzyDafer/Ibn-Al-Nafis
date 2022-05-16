using GUI.Models.Entities;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IAdmissionsRepository
    {
        #region GetMethods
        Task<Dictionary<long, Admission>> Admin_Select_Admissions_by_date(DateTime Date);
        Task<Dictionary<long, Admission>> Admin_Select_Admissions();
        Task<Dictionary<long, Admission>> Admin_Select_Admissions_by_Clinic(string Clinic);
        Task<Dictionary<long, Admission>> Doctor_Select_Admissions_by_clinic(string Clinic);
        Task<Admission> Doctor_Select_Admission_by_ID(long ID);
        Task<Dictionary<long, Admission>> Doctor_Select_Admissions();
        Task<Dictionary<long, Admission>> Reception_Select_Registered();
        #endregion

        #region Operation
        Task<StoredResult> Reception_Add_Admission(Admission Admission);
        Task<StoredResult> Doctor_Edit_Admission(Admission Admission);
        #endregion
    }
}
