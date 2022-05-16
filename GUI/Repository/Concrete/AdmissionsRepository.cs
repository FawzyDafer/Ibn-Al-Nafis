using GUI.Models.Entities;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class AdmissionsRepository : GenericRepo, IAdmissionsRepository
    {
        #region Constructors
        public AdmissionsRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<long, Admission>> Admin_Select_Admissions() =>
            await SetDataTableInDMCol("Admin_Select_Admissions", CMD);

        public async Task<Dictionary<long, Admission>>
            Admin_Select_Admissions_by_date(DateTime Date) =>
            await SetDataTableInDMCol(
                "Admin_Select_Admissions_by_date",
                AddParameters(new SqlParameter("@Date", Date)));

        public async Task<Dictionary<long, Admission>>
            Doctor_Select_Admissions_by_clinic(string Clinic) =>
            await SetDataTableInDMCol(
                "Doctor_Select_Admissions_by_clinic",
                AddParameters(new SqlParameter("@Clinic", Clinic)));

        public async Task<Admission> Doctor_Select_Admission_by_ID(long ID)
        {
            var objDMCol = await SetDataTableInDMCol(
                "Doctor_Select_Admission_by_ID",
                AddParameters(new SqlParameter("@AdmissionID", ID)));
            return objDMCol.Values.FirstOrDefault();
        }

        public async Task<Dictionary<long, Admission>>
            Admin_Select_Admissions_by_Clinic(string Clinic) =>
            await SetDataTableInDMCol(
                "Admin_Select_Admissions_by_Clinic",
                AddParameters(new SqlParameter("@Clinic", Clinic)));

        public async Task<Dictionary<long, Admission>> Doctor_Select_Admissions() =>
            await SetDataTableInDMCol("Doctor_Select_Admissions", CMD);

        public async Task<Dictionary<long, Admission>> Reception_Select_Registered() =>
            await SetDataTableInDMCol("Reception_Select_Registered", CMD);

        #endregion

        #region OperationMethods
        public async Task<StoredResult> Doctor_Edit_Admission(Admission Admission)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Doctor_Edit_Admission", AddParameters(
            new SqlParameter[] {
                new SqlParameter("@AdmissionID", Admission.Counter),
                    outputparameter
                }));
            string id = outputparameter.Value.ToString();
            return await Check(id, null, Admission.Counter);
        }

        public async Task<StoredResult> Reception_Add_Admission(Admission Admission)
        {
            SqlParameter outputparameter = Outputparameter;
            long AdmissionID = await Insert("Reception_Add_Admission", AddParameters(
                new SqlParameter[] {
                    new SqlParameter("@PatientID", Admission.PatientsSSN),
                    new SqlParameter("@Date", Admission.VisitDate),
                    new SqlParameter("@Emergency", Admission.Emergency),
                    new SqlParameter("@State", Admission.State),
                    new SqlParameter("@Clinic", Admission.Clinic),
                    outputparameter
                }));
            string id = outputparameter.Value.ToString();
            return await Check(id, null, AdmissionID);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<long, Admission>>
            SetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var Admissions = await GetListData<Admission>(StoredProcedure, cmd);
            return Admissions.ToDictionary(Key => Key.Counter);
        }
        #endregion
    }
}
