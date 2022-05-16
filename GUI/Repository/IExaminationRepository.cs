using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IExaminationRepository
    {
        #region GetMethods
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_All_PatientsExamination();
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_Clinic_PatientsExamination(string clinic);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_Description_PatientsExamination(string Description);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_DoctorID_PatientsExamination(string DoctorID);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_ExaminationID_PatientsExamination(string ExaminationID);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_Emergency_PatientsExamination(bool Emergency);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_PatientSSN_PatientsExamination(string PatientSSN);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_Problem_PatientsExamination(string Problem);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_VisitDate_PatientsExamination(DateTime VisitDate);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_AddmissionID_PatientsExamination(long addmissionID);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_CategoryID_PatientsExamination(string CategoryID);
        Task<Dictionary<long, PatientsExaminations>> Examination_Select_By_Category_PatientsExamination(string Category);
        #endregion

        #region Operation Methods
        Task<StoredResult> Examination_Add_Examination(Examination Examination);
        Task<StoredResult> Examination_Edit_Examination(Examination Examination);
        Task<StoredResult> Examination_Add_PatientsExamination(PatientExamination patientExamination);
        Task<StoredResult> Examination_Edit_PatientsExamination(PatientExamination patientExamination);
        #endregion
    }
}
