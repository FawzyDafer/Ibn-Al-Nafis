using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface ICompanyRepository
    {
        #region GetMethods
        Task<PatientsRelatives> Reception_Select_Company_by_CompanyID(string RelativeID);
        Task<Dictionary<string, PatientsRelatives>> Reception_Select_Company_by_Date(DateTime Date);
        Task<Dictionary<string, PatientsRelatives>> Reception_Select_Company_by_Name(string Name);
        Task<Dictionary<string, PatientsRelatives>> Reception_Select_Company_by_PatientID(string PatientID);
        Task<Dictionary<string, PatientsRelatives>> Reception_Select_Company_by_Phone(string phone);
        #endregion

        #region OperationMethods
        Task<StoredResult> Reception_Add_Company(Person Relative);
        Task<StoredResult> Reception_Add_PatientCompany(PatientRelative Relative);
        Task<StoredResult> Reception_Edit_Company(Person Relative);
        #endregion
    }
}
