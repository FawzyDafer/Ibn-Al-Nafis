using GUI.Models.Entities;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class ClinicRepository : GenericRepo, IClinicRepository
    {
        #region Constructors
        public ClinicRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<string, Clinic>> Reception_Select_Clinic_by_Day() =>
            await ClinicSetDataTableInDMCol("Reception_Select_Clinic_by_Day", CMD);

        public async Task<Dictionary<string, Clinic>> Admin_Select_Clinics() =>
            await ClinicSetDataTableInDMCol("Admin_Select_Clinics", CMD);

        public async Task<List<ClinicsDay>> Admin_Select_ClinicDays() =>
            await GetListData<ClinicsDay>("Reception_Select_Clinic_by_Day", CMD);

        public async Task<List<ClinicsDay>> Admin_Select_ClinicDays_by_Clinic(string Clinic)
            => await GetListData<ClinicsDay>(
                "Admin_Select_ClinicDays_by_Clinic",
                AddParameters(new SqlParameter("@Clinic", Clinic)));
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Admin_Add_ClinicDays(ClinicsDay ClinicsDay)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Admin_Add_ClinicDays", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@Clinic", ClinicsDay.Clinic),
                new SqlParameter("@Day", ClinicsDay.DayID),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, ClinicsDay.Clinic);
        }

        public async Task<StoredResult> Admin_Delete_ClinicDays_by_Clinic(string Clinic)
        {
            bool Result = await ExecuteQuery(
                "Admin_Delete_ClinicDays_by_Clinic", AddParameters(
                    new SqlParameter("@Clinic", Clinic)));
            StoredResult result = new StoredResult
            {
                Success = Result,
                ID = Clinic,
                ErrorMessage = (Result) ? null : "There is an error ocure while trying to perform your request.please try again."
            };
            return result;
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<string, Clinic>>
            ClinicSetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var clinics = await GetListData<Clinic>(StoredProcedure, cmd);
            return clinics.ToDictionary(key => key.Title);
        }
        #endregion
    }
}
