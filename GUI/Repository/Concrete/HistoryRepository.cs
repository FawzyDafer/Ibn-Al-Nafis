using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class HistoryRepository : GenericRepo, IHistoryRepository
    {
        #region Constructors
        public HistoryRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_All_PatientsHistory() =>
            await GetDictionaryData<PatientsHistories>("History_Select_All_PatientsHistory", CMD);

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Clinic_PatientsHistory(string clinic) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_Clinic_PatientsHistory",
                AddParameters(new SqlParameter("@Clinic", clinic)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Description_PatientsHistory(string Description) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_Description_PatientsHistory",
                AddParameters(new SqlParameter("@Description", Description)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_DoctorID_PatientsHistory(string DoctorID) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_DoctorID_PatientsHistory",
                AddParameters(new SqlParameter("@DoctorID", DoctorID)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_HistoryID_PatientsHistory(string HistoryID) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_HistoryID_PatientsHistory",
                AddParameters(new SqlParameter("@HistoryID", HistoryID)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Emergency_PatientsHistory(bool Emergency) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_Emergency_PatientsHistory",
                AddParameters(new SqlParameter("@Emergincy", Emergency)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_PatientSSN_PatientsHistory(string PatientSSN) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_PatientSSN_PatientsHistory",
                AddParameters(new SqlParameter("@PatientSSN", PatientSSN)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Problem_PatientsHistory(string Problem) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_Problem_PatientsHistory",
                AddParameters(new SqlParameter("@Problem", Problem)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_VisitDate_PatientsHistory(DateTime VisitDate) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_VisitDate_PatientsHistory",
                AddParameters(new SqlParameter("@VisitDate", VisitDate)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_AddmissionID_PatientsHistory(long addmissionID) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_AddmissionID_PatientsHistory",
                AddParameters(new SqlParameter("@AddminssionID", addmissionID)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_CategoryID_PatientsHistory(string CategoryID) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_CategoryID_PatientsHistory",
                AddParameters(new SqlParameter("@CategoryID", CategoryID)));

        public async Task<Dictionary<long, PatientsHistories>>
            History_Select_By_Category_PatientsHistory(string Category) =>
            await GetDictionaryData<PatientsHistories>("History_Select_By_Category_PatientsHistory",
                AddParameters(new SqlParameter("@Category", Category)));
        #endregion

        #region Operation Methods
        public async Task<StoredResult> History_Add_History(History history)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Add_History", GetParameter(history, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, history.HistoryID);
        }
        public async Task<StoredResult> History_Edit_History(History history)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Edit_History", GetParameter(history, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, history.HistoryID);
        }
        public async Task<StoredResult> History_Add_PatientsHistory
            (PatientHistory patientHistory)
        {
            var outputParameter = Outputparameter;
            long AdmissionID = await Insert("History_Add_PatientsHistory",
                GetParameter(patientHistory, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, AdmissionID);
        }
        public async Task<StoredResult> History_Edit_PatientsHistory
            (PatientHistory patientHistory)
        {
            var outputParameter = Outputparameter;
            long AdmissionID = await Insert("History_Edit_PatientsHistory",
                GetParameter(patientHistory, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, AdmissionID);
        }
        #endregion

        #region Private Methods
        SqlCommand GetParameter(PatientHistory patientHistory, SqlParameter parameter)
            => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@HistoryID", patientHistory.HistoryID),
                new SqlParameter("@AdmissionID", patientHistory.AdmissionID),
                new SqlParameter("@Problem", patientHistory.Problem),
                new SqlParameter("@DoctorID", patientHistory.DoctorID),
                new SqlParameter("@Notes", patientHistory.Note),
                parameter
            });
        SqlCommand GetParameter(History history, SqlParameter parameter)
            => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@HistoryID", history.HistoryID),
                new SqlParameter("@Description", history.Description),
                new SqlParameter("@Category", history.Category),
                parameter
            });
        #endregion
    }
}
