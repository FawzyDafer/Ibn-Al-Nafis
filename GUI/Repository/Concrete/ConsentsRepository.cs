using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class ConsentsRepository : GenericRepo, IConsentsRepository
    {
        #region Constructors
        public ConsentsRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<long, PatientsConsents>>
            Reception_Select_Consents() =>
            await SetDataTableInDMCol("Reception_Select_Consents", CMD);

        public async Task<Dictionary<long, PatientsConsents>>
            Reception_Select_Consents_by_Admission(long AdmissionID) =>
            await SetDataTableInDMCol("Reception_Select_Consents_by_Admission",
                AddParameters(new SqlParameter("@AdmissionID", AdmissionID)));

        public async Task<Dictionary<long, PatientsConsents>>
            Reception_Select_Consents_by_Consent(string Consent) =>
            await SetDataTableInDMCol("Reception_Select_Consents_by_Consent",
                AddParameters(new SqlParameter("@Consent", Consent)));

        public async Task<Dictionary<long, PatientsConsents>>
            Reception_Select_Consents_by_Patient(string SSN) =>
            await SetDataTableInDMCol("Reception_Select_Consents_by_Patient",
                AddParameters(new SqlParameter("@SSN", SSN)));

        public async Task<PatientsConsents>
            Reception_Select_Consents_by_ID(string PCID)
        {
            var objDMCol = await SetDataTableInDMCol(
                "Reception_Select_Consents_by_ID",
                AddParameters(new SqlParameter("@PCID", PCID)));
            return objDMCol.Values.FirstOrDefault();
        }

        public async Task<Dictionary<string, Consent>> Reception_Select_GetConsents() =>
            await SetDataConTableInDMCol("Reception_Select_GetConsents", CMD);

        public async Task<Consent> Reception_Select_GetConsents_by_ID(string ConID)
        {
            var objDMCol = await SetDataConTableInDMCol(
                "Reception_Select_GetConsents_by_ID",
                AddParameters(new SqlParameter("@ConID", ConID)));
            return objDMCol.FirstOrDefault().Value;
        }
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Reception_Add_Consent(Consent Consent)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Reception_Add_Consent", AddParameters(
                GetSqlParameters(Consent, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, Consent.ConID);
        }

        public async Task<StoredResult> Reception_Edit_Consent(Consent Consent)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Reception_Edit_Consent", AddParameters(
                GetSqlParameters(Consent, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, Consent.ConID);
        }

        public async Task<StoredResult> Reception_Add_PatientConsent(PatientConsent PatientConsent)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Reception_Add_PatientConsent", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@ConID", PatientConsent.ConID),
                new SqlParameter("@PCID", PatientConsent.PCID),
                new SqlParameter("@ImageData", PatientConsent.ImageData),
                new SqlParameter("@ImageExtention", PatientConsent.ImageExtention),
                outputparameter,
                new SqlParameter("@AdmissionID", PatientConsent.AdmissionID)
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, PatientConsent.ConID);
        }

        public async Task<StoredResult> Reception_Edit_PatientConsent(PatientConsent PatientConsent)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Reception_Edit_PatientConsent", AddParameters(new SqlParameter[] {
                new SqlParameter("@ConID", PatientConsent.ConID),
                new SqlParameter("@PCID", PatientConsent.PCID),
                new SqlParameter("@ImageData", PatientConsent.ImageData),
                new SqlParameter("@ImageExtention", PatientConsent.ImageExtention),
                outputparameter,
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, PatientConsent.ConID);
        }
        #endregion

        #region Private Methods
        SqlParameter[] GetSqlParameters(Consent Consent, SqlParameter parameter) => new SqlParameter[]
        {
            new SqlParameter("@ConID", Consent.ConID),
            new SqlParameter("@Description", Consent.Description),
            parameter
        };

        async Task<Dictionary<long, PatientsConsents>>
            SetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var Consents = await GetListData<PatientsConsents>(StoredProcedure, cmd);
            return Consents.ToDictionary(key => key.AdmissionID);
        }

        async Task<Dictionary<string, Consent>>
            SetDataConTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var Consents = await GetListData<Consent>(StoredProcedure, cmd);
            return Consents.ToDictionary(key => key.ConID);
        }
        #endregion
    }
}
