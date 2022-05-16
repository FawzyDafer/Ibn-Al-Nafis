using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IMedicineRepository
    {
        #region Get Methods
        Task<Dictionary<long, PatientsMedicines>> History_Select_All_Medicines();
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_AddmissionID_Medicines(long AdmissionID);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_AlternativeID_Medicines(string AlternativeID);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_DoctorID_Medicines(string DoctorID);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_EffectiveMaterial_Medicines(string EffectiveMaterial);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_MedicineID_Medicines(string MedicineID);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_PatientSSN_Medicines(string PatientSSN);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_VisitDate_Medicines(DateTime VisitDate);
        Task<Dictionary<long, PatientsMedicines>> History_Select_By_VisitDate_Medicines(string Medicine);
        #endregion

        #region Operation Methods
        Task<StoredResult> History_Add_Medicines(Medicine history);
        Task<StoredResult> History_Add_PatientsMedicines(PatientMedicine patientHistory);
        Task<StoredResult> History_Edit_Medicines(Medicine history);
        #endregion
    }
}
