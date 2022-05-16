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
    public class MedicineRepository : GenericRepo, IMedicineRepository
    {
        #region Constructors
        public MedicineRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region Get Methods
        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_All_Medicines() =>
             await GetDictionaryData<PatientsMedicines>("History_Select_All_Medicines", CMD);
        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_AddmissionID_Medicines(long AdmissionID) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_AddmissionID_Medicines",
                AddParameters(new SqlParameter("@AddmissionID", AdmissionID)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_AlternativeID_Medicines(string AlternativeID) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_AlternativeID_Medicines",
                AddParameters(new SqlParameter("@AlternativeID", AlternativeID)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_DoctorID_Medicines(string DoctorID) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_DoctorID_Medicines",
                AddParameters(new SqlParameter("@DoctorID", DoctorID)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_EffectiveMaterial_Medicines(string EffectiveMaterial) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_EffectiveMaterial_Medicines",
                AddParameters(new SqlParameter("@EffectiveMaterial", EffectiveMaterial)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_MedicineID_Medicines(string MedicineID) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_MedicineID_Medicines",
                AddParameters(new SqlParameter("@MedicineID", MedicineID)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_PatientSSN_Medicines(string PatientSSN) =>
            await GetDictionaryData<PatientsMedicines>("History_Select_By_PatientSSN_Medicines",
                AddParameters(new SqlParameter("@PatientSSN", PatientSSN)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_VisitDate_Medicines(DateTime VisitDate) =>
             await GetDictionaryData<PatientsMedicines>("History_Select_By_VisitDate_Medicines",
                AddParameters(new SqlParameter("@VisitDate", VisitDate)));

        public async Task<Dictionary<long, PatientsMedicines>>
            History_Select_By_VisitDate_Medicines(string Medicine) =>
            await GetDictionaryData<PatientsMedicines>("Medicine_select_by_Name",
                AddParameters(new SqlParameter("@Medicine", Medicine)));
        #endregion

        #region Operation Methods
        public async Task<StoredResult> History_Add_Medicines(Medicine medicine)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Add_Medicine", AddParameters(
                GetParameters(medicine, outputParameter)));
            string id = outputParameter.Value.ToString();
            return await Check(id, medicine.MedicineName);
        }

        public async Task<StoredResult>
            History_Add_PatientsMedicines(PatientMedicine patientMedicine)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Add_PatientMedicine", AddParameters(
                GetParameters2(patientMedicine, outputParameter)));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, patientMedicine.AdmissionID);
        }

        public async Task<StoredResult>
            History_Edit_PatientMedicine(PatientMedicine patientMedicine)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Edit_PatientMedicine", AddParameters(
                GetParameters2(patientMedicine, outputParameter)));
            string id = outputParameter.Value.ToString();
            return await Check(id, null, patientMedicine.AdmissionID);
        }

        public async Task<StoredResult> History_Edit_Medicines(Medicine medicine)
        {
            SqlParameter outputParameter = Outputparameter;
            await ExecuteQuery("History_Edit_Medicines", AddParameters(
                GetParameters(medicine, outputParameter)));
            string id = outputParameter.Value.ToString();
            return await Check(id, medicine.MedicineName);
        }
        #endregion

        #region private Methods 
        SqlParameter[] GetParameters(Medicine medicine, SqlParameter parameter)
            => new SqlParameter[] {
                new SqlParameter("@MedicineID", medicine.MedicineID),
                new SqlParameter("@Medicine", medicine.MedicineName),
                new SqlParameter("@AlternativeMedicineID", medicine.AlternativeID),
                new SqlParameter("@EffectiveMaterial", medicine.EffectiveMaterial),
                parameter
            };

        SqlParameter[] GetParameters2(PatientMedicine Medicine, SqlParameter parameter)
            => new SqlParameter[] {
                    new SqlParameter("@AdmissionID", Medicine.AdmissionID),
                    new SqlParameter("@MedicineID", Medicine.MedicineID),
                    new SqlParameter("@DoctorID", Medicine.DoctorID),
                    new SqlParameter("@Dose", Medicine.Dose),
                    new SqlParameter("@Frequency", Medicine.Frequency),
                    new SqlParameter("@ReasonIfKnown", Medicine.ReasonIfKnown),
                    new SqlParameter("@Continue", Medicine.Continue),
                    parameter
            };
        #endregion
    }
}
