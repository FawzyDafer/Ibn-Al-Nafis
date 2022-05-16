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
    public class PainAssissmentRepository : GenericRepo, IPainAssissmentRepository
    {
        #region Constructors
        public PainAssissmentRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<PatientsPains> History_Select_Pain_By_AdmissionID(long AdmissionID)
        {
            var Pains = await SetDataTableInDMCol("History_Select_Pain_By_AdmissionID",
                   AddParameters(new SqlParameter("@AdmissionID", AdmissionID)));
            return Pains.Values.FirstOrDefault();
        }

        public async Task<Dictionary<string, PatientsPains>>
            History_Select_Pain_By_PatientsSSN(string PatientsSSN) =>
            await SetDataTableInDMCol("History_Select_Pain_By_PatientsSSN",
                AddParameters(new SqlParameter("@PatientsSSN", PatientsSSN)));

        public async Task<PatientsPains> History_Select_Pain_By_PainID(string PainID)
        {
            var Pains = await SetDataTableInDMCol("History_Select_Pain_By_PainID",
                AddParameters(new SqlParameter("@PainID", PainID)));
            return Pains.Values.FirstOrDefault();
        }
        #endregion

        #region Operation Methods
        public async Task<StoredResult> History_Add_Assissment(PainAssissment Pain)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Add_Assissment", GetParameter(Pain, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, Pain.PainID);
        }
        public async Task<StoredResult> History_Edit_Assissment(PainAssissment Pain)
        {
            SqlParameter outputParameter = Outputparameter;
            await Insert("History_Edit_Assissment", GetParameter(Pain, outputParameter));
            string id = outputParameter.Value.ToString();
            return await Check(id, Pain.PainID);
        }
        #endregion

        #region Private Methods
        SqlCommand GetParameter(PainAssissment Pain, SqlParameter parameter)
            => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@PainID", Pain.PainID),
                new SqlParameter("@AdmissionID", Pain.AdmissionID),
                new SqlParameter("@HasPain", Pain.HasPain),
                new SqlParameter("@Rate", Pain.Rate),
                new SqlParameter("@Location", Pain.Location),
                new SqlParameter("@Character", Pain.Character),
                new SqlParameter("@Frequency", Pain.Frequency),
                new SqlParameter("@Relieves", Pain.Relieves),
                new SqlParameter("@Referral", Pain.Referral),
                new SqlParameter("@Duration", Pain.Duration),
                new SqlParameter("@Aggravates", Pain.Aggravates),
                new SqlParameter("@PastPain", Pain.PastPain),
                new SqlParameter("@Impact", Pain.Impact),
                new SqlParameter("@Therapies", Pain.Therapies),
                parameter
            });

        async Task<Dictionary<string, PatientsPains>>
            SetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var Admissions = await GetListData<PatientsPains>(StoredProcedure, cmd);
            return Admissions.ToDictionary(Key => Key.PainID);
        }
        #endregion
    }
}
