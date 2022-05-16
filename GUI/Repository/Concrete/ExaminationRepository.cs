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
    public class ExaminationRepository : GenericRepo, IExaminationRepository
    {
        #region Constructors
        public ExaminationRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_All_PatientsExamination() =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_All_PatientsExamination", CMD);

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_Clinic_PatientsExamination(string clinic) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_Clinic_PatientsExamination",
                AddParameters(new SqlParameter("@Clinic", clinic)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_Description_PatientsExamination(string Description) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_Description_PatientsExamination",
                AddParameters(new SqlParameter("@Description", Description)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_DoctorID_PatientsExamination(string DoctorID) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_DoctorID_PatientsExamination",
                AddParameters(new SqlParameter("@DoctorID", DoctorID)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_ExaminationID_PatientsExamination(string ExaminationID) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_ExaminationID_PatientsExamination",
                AddParameters(new SqlParameter("@ExaminationID", ExaminationID)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_Emergency_PatientsExamination(bool Emergency) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_Emergency_PatientsExamination",
                AddParameters(new SqlParameter("@Emergincy", Emergency)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_PatientSSN_PatientsExamination(string PatientSSN) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_PatientSSN_PatientsExamination",
                AddParameters(new SqlParameter("@PatientSSN", PatientSSN)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_Problem_PatientsExamination(string Problem) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_Problem_PatientsExamination",
                AddParameters(new SqlParameter("@Problem", Problem)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_VisitDate_PatientsExamination(DateTime VisitDate) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_VisitDate_PatientsExamination",
                AddParameters(new SqlParameter("@VisitDate", VisitDate)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_AddmissionID_PatientsExamination(long addmissionID) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_AddmissionID_PatientsExamination",
                AddParameters(new SqlParameter("@AddminssionID", addmissionID)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_CategoryID_PatientsExamination(string CategoryID) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_CategoryID_PatientsExamination",
                AddParameters(new SqlParameter("@CategoryID", CategoryID)));

        public async Task<Dictionary<long, PatientsExaminations>>
            Examination_Select_By_Category_PatientsExamination(string Category) =>
            await GetDictionaryData<PatientsExaminations>("Examination_Select_By_Category_PatientsExamination",
                AddParameters(new SqlParameter("@Category", Category)));
        #endregion

        #region Operation Methods
        public async Task<StoredResult> Examination_Add_Examination(Examination Examination)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("Examination_Add_Examination", GetParameter(Examination, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, Examination.ExaminationID);
        }
        public async Task<StoredResult> Examination_Edit_Examination(Examination Examination)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("Examination_Edit_Examination", GetParameter(Examination, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, Examination.ExaminationID);
        }
        public async Task<StoredResult> Examination_Add_PatientsExamination
            (PatientExamination patientExamination)
        {
            var outputParameter = Outputparameter;
            long AdmissionID = await Insert("Examination_Add_PatientsExamination",
                GetParameter(patientExamination, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, AdmissionID);
        }
        public async Task<StoredResult> Examination_Edit_PatientsExamination
            (PatientExamination patientExamination)
        {
            var outputParameter = Outputparameter;
            long AdmissionID = await Insert("Examination_Edit_PatientsExamination",
                GetParameter(patientExamination, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, AdmissionID);
        }
        #endregion

        #region Private Methods
        SqlCommand GetParameter(PatientExamination patientExamination, SqlParameter parameter)
            => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@ExaminationID", patientExamination.ExaminationID),
                new SqlParameter("@AdmissionID", patientExamination.AdmissionID),
                new SqlParameter("@Problem", patientExamination.Problem),
                new SqlParameter("@DoctorID", patientExamination.DoctorID),
                new SqlParameter("@Notes", patientExamination.Note),
                parameter
            });
        SqlCommand GetParameter(Examination Examination, SqlParameter parameter)
            => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@ExaminationID", Examination.ExaminationID),
                new SqlParameter("@Description", Examination.Description),
                new SqlParameter("@Category", Examination.Category),
                parameter
            });
        #endregion
    }
}
