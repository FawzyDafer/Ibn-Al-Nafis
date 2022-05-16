using GUI.Models.Entities;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class PatientRepository : GenericRepo, IPatientRepository
    {
        #region Constructors
        public PatientRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<string, Patient>>
            Reception_Select_Patient_by_Name(string Name) =>
            await SetDataTableInDMCol(
                "Reception_Select_Patient_by_Name",
                AddParameters(new SqlParameter("@Name", Name)));

        public async Task<Dictionary<string, Patient>>
            Reception_Select_Patient_by_Phone(string Phone) =>
            await SetDataTableInDMCol(
                "Reception_Select_Patient_by_Name",
                AddParameters(new SqlParameter("@phone", Phone)));

        public async Task<Patient>
            Reception_Select_Patient_by_SSN(string SSN)
        {
            var objDMCol = await SetDataTableInDMCol(
                "Reception_Select_Patient_by_SSN",
                AddParameters(new SqlParameter("@SSN", SSN)));
            return objDMCol.Values.FirstOrDefault();
        }
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Reception_Add_Patient(Patient Patient)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Reception_Add_Patient", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@SSN", Patient.SSN),
                new SqlParameter("@Name", Patient.Name),
                new SqlParameter("@Sex", Patient.Sex),
                new SqlParameter("@DateOfBirth", Patient.DateOfBirth),
                new SqlParameter("@Phone", Patient.Phone),
                new SqlParameter("@Address", Patient.Address),
                new SqlParameter("@Language", Patient.Language),
                new SqlParameter("@RequiredTranslator", Patient.RequiredTranslator),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Patient.SSN);
        }

        public async Task<StoredResult> Reception_Edit_Patient(Patient Patient)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Reception_Edit_Patient", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@SSN", Patient.SSN),
                new SqlParameter("@Phone", Patient.Phone),
                new SqlParameter("@Address", Patient.Address),
                new SqlParameter("@MariedStatus", Patient.MariedStatus),
                new SqlParameter("@WorkingStatus", Patient.WorkingStatus),
                new SqlParameter("@HasChildren", Patient.HasChildren),
                new SqlParameter("@Language", Patient.Language),
                new SqlParameter("@RequiredTranslator", Patient.RequiredTranslator),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Patient.SSN);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<string, Patient>>
            SetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var patients = await GetListData<Patient>(StoredProcedure, cmd);
            return patients.ToDictionary(key => key.SSN);
        }
        #endregion
    }
}
