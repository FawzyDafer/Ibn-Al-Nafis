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
    public class FollowUpRepository : GenericRepo, IFollowUpRepository
    {
        #region Constructors
        public FollowUpRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<string, PatientsFollowUps>>
            Reception_Select_Patient_by_FllowUpdate() =>
            await SetDataTableInDMCol("Reception_Select_Patient_by_FllowUpdate", CMD);

        public async Task<Dictionary<string, PatientsFollowUps>>
            Doctor_Select_FoolowUp_by_Admission(long AdmissionID) =>
            await SetDataTableInDMCol("Doctor_Select_FoolowUp_by_Admission",
                AddParameters(new SqlParameter("@AdmisssionID", AdmissionID)));
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Doctor_Add_NewFollowUp(FllowUp FllowUp)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Doctor_Add_NewFollowUp", GetParameters(FllowUp, outputparameter));
            string id = outputparameter.Value.ToString();
            return await Check(id, FllowUp.FllowUpID);
        }

        public async Task<StoredResult> Doctor_Edit_NewFollowUp(FllowUp FllowUp)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Doctor_Edit_NewFollowUp", GetParameters(FllowUp, outputparameter));
            string id = outputparameter.Value.ToString();
            return await Check(id, FllowUp.FllowUpID);
        }

        public async Task<StoredResult> Reception_Edit_FollowUp(long Admission)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Reception_Edit_FollowUp", AddParameters(new SqlParameter[] {
                    new SqlParameter("@AdmissionID", Admission),
                    outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, null, Admission);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<string, PatientsFollowUps>> SetDataTableInDMCol
            (string StoredProcedure, SqlCommand cmd)
        {
            var Patients = await GetListData<PatientsFollowUps>(StoredProcedure, cmd);
            Dictionary<string, PatientsFollowUps> objDMCol = new Dictionary<string, PatientsFollowUps>();
            await Task.Factory.StartNew(() =>
            {
                foreach (var item in Patients)
                {
                    try
                    {
                        objDMCol.Add(item.SSN, item);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            return objDMCol;
        }

        SqlCommand GetParameters(FllowUp FllowUp, SqlParameter outputparameter) =>
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("@FlowUpID", FllowUp.FllowUpID),
                new SqlParameter("@FllowupReason", FllowUp.FllowupReason),
                new SqlParameter("@FollowUpEnd", FllowUp.FollowUpEnd),
                new SqlParameter("@FollowUpBegin", FllowUp.FollowUpBegin),
                new SqlParameter("@FollowUp", FllowUp.FollowUp),
                new SqlParameter("@AdmisssionID", FllowUp.AdmisssionID),
                new SqlParameter("@DoctorID", FllowUp.DoctorID),
                outputparameter
            });
        #endregion
    }
}
