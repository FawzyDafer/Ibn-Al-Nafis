using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class DischargeRepository : GenericRepo, IDischargeRepository
    {
        #region Constructors
        public DischargeRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region Get Methods
        public async Task<Dictionary<long, PatientsDischarges>>
            History_Select_All_Discharge() =>
            await SetDataTableInDMCol("History_SelectAlll_Discharge", CMD);

        public async Task<Dictionary<long, PatientsDischarges>>
            History_Select_By_Date_Discharge(DateTime dateTime) =>
            await SetDataTableInDMCol("History_SelectByDate_Discharge",
                AddParameters(new SqlParameter("@DateTime", dateTime)));

        public async Task<Dictionary<long, PatientsDischarges>>
            History_Select_By_PatientSSN_Discharge(string PatientSSN) =>
            await SetDataTableInDMCol("History_SelectByPatientSSN_Discharge",
                AddParameters(new SqlParameter("@PatientsSSN", PatientSSN)));

        public async Task<Dictionary<long, PatientsDischarges>>
            History_Select_By_State_Discharge(string State) =>
            await SetDataTableInDMCol("History_SelectByState_Discharge",
                AddParameters(new SqlParameter("@State", State)));

        #endregion

        #region Operation Methods
        public async Task<StoredResult> History_Add_Discharge(Discharge discharge)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("History_Add_Discharge", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@AddmissionID", discharge.AdmissionID),
                new SqlParameter("@Discharge", discharge.ISDischarge),
                new SqlParameter("@DateTime", discharge.DateTime),
                new SqlParameter("@State", discharge.Statee),
                new SqlParameter("@DischargeSummary", discharge.DischargeSummary),
                new SqlParameter("@DoctorID", discharge.DoctorID),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, null, discharge.AdmissionID);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<long, PatientsDischarges>> SetDataTableInDMCol
            (string StoredProcedure, SqlCommand cmd)
        {
            var discharges = await GetListData<PatientsDischarges>(StoredProcedure, cmd);
            return discharges.ToDictionary(x => x.AdmissionID);
        }
        #endregion
    }
}
