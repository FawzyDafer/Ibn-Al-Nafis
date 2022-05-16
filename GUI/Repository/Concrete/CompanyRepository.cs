using GUI.Models;
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
    public class CompanyRepository : GenericRepo, ICompanyRepository
    {
        #region Constructors
        public CompanyRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<PatientsRelatives>
            Reception_Select_Company_by_CompanyID(string RelativeID)
        {
            var objDMCol = await SetDataTableInDMCol(
                "Reception_Select_Company_by_CompanyID",
                AddParameters(new SqlParameter("@SSN", RelativeID)));
            return objDMCol.Values.FirstOrDefault();
        }

        public async Task<Dictionary<string, PatientsRelatives>>
            Reception_Select_Company_by_Date(DateTime Date) =>
            await SetDataTableInDMCol(
                "Reception_Select_Company_by_Date",
                AddParameters(new SqlParameter("@Date", Date)));

        public async Task<Dictionary<string, PatientsRelatives>>
            Reception_Select_Company_by_PatientID(string PatientID) =>
            await SetDataTableInDMCol(
                "Reception_Select_Company_by_PatientID",
                AddParameters(new SqlParameter("@SSN", PatientID)));

        public async Task<Dictionary<string, PatientsRelatives>>
            Reception_Select_Company_by_Name(string Name) =>
            await SetDataTableInDMCol(
                "Reception_Select_Company_by_Name",
                AddParameters(new SqlParameter("@Name", Name)));

        public async Task<Dictionary<string, PatientsRelatives>>
            Reception_Select_Company_by_Phone(string phone) =>
            await SetDataTableInDMCol(
                "Reception_Select_Company_by_Phone",
                AddParameters(new SqlParameter("@Phone", phone)));
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Reception_Add_Company(Person Company)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Reception_Add_Company", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@SSN", Company.SSN),
                new SqlParameter("@Phone", Company.Phone),
                new SqlParameter("@Address", Company.Address),
                outputparameter,
                new SqlParameter("@Name", Company.Name),
                new SqlParameter("@Sex", Company.Sex)
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Company.SSN);
        }

        public async Task<StoredResult> Reception_Edit_Company(Person Company)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Reception_Edit_Company", AddParameters(new SqlParameter[] {
                new SqlParameter("@SSN", Company.SSN),
                new SqlParameter("@Phone", Company.Phone),
                new SqlParameter("@Address", Company.Address),
                outputparameter,
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Company.SSN);
        }

        public async Task<StoredResult>
            Reception_Add_PatientCompany(PatientRelative Relative)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Reception_Add_PatientCompany", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@AdmissionID", Relative.AdmissionID),
                new SqlParameter("@CompanyID", Relative.RelativeSSN),
                new SqlParameter("@Relation", Relative.Relation),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Relative.RelativeSSN);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<string, PatientsRelatives>>
            SetDataTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var Companies = await GetListData<PatientsRelatives>(StoredProcedure, cmd);
            Dictionary<string, PatientsRelatives> objDMCol = new Dictionary<string, PatientsRelatives>();
            await Task.Factory.StartNew(() =>
            {
                foreach (var item in Companies)
                {
                    try
                    {
                        objDMCol.Add(item.RelativeSSN, item);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            return objDMCol;
        }
        #endregion
    }
}
