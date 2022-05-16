using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IHistoryRepository
    {
        #region Get Methods
        Task<Dictionary<long, PatientsHistories>>
            History_Select_All_PatientsHistory();
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Clinic_PatientsHistory(string clinic);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Description_PatientsHistory(string Description);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_DoctorID_PatientsHistory(string DoctorID);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_AddmissionID_PatientsHistory(long addmissionID);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Emergency_PatientsHistory(bool Emergency);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_HistoryID_PatientsHistory(string HistoryID);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_PatientSSN_PatientsHistory(string PatientSSN);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Problem_PatientsHistory(string Type);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_VisitDate_PatientsHistory(DateTime VisitDate);
        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_CategoryID_PatientsHistory(string CategoryID);

        Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Category_PatientsHistory(string Category);
        #endregion

        #region Operation Methods
        Task<StoredResult> History_Add_History(History history);
        Task<StoredResult> History_Edit_History(History history);
        Task<StoredResult> History_Add_PatientsHistory(PatientHistory patientHistory);
        Task<StoredResult> History_Edit_PatientsHistory(PatientHistory patientHistory);
        #endregion
    }
}
