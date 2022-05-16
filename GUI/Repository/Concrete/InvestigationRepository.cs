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
    public class InvestigationRepository : GenericRepo, IInvestigationRepository
    {
        #region Constructors
        public InvestigationRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigations() =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigations", CMD);

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_PatientSSN(string PatientSSN, string Sort) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_PatientSSN",
                AddParameters(new SqlParameter[]{
                    new SqlParameter("@PatientSSN", PatientSSN),
                    new SqlParameter("@Sort", Sort)
                }));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_Clinic(string Clinic) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_Clinic",
                AddParameters(new SqlParameter("@Clinic", Clinic)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_Type(string Type) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_Type",
                AddParameters(new SqlParameter("@Type", Type)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_Category(string Category) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_Category",
                AddParameters(new SqlParameter("@Category", Category)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_Sort(string Sort) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_Sort",
                AddParameters(new SqlParameter("@Sort", Sort)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_InvesigationCategory_by_Category(string Category) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_InvesigationCategory_by_Category",
                AddParameters(new SqlParameter("@Category", Category)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_InvesigationCategory_by_Sort(string Sort) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_InvesigationCategory_by_Sort",
                AddParameters(new SqlParameter("@Sort", Sort)));

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_InvesigationCategory_by_Type(string Type) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_InvesigationCategory_by_Type",
                AddParameters(new SqlParameter("@Type", Type)));

        public async Task<PatientsInvestigations>
            Investigation_Select_Invesigation_by_TypeID(string TypeID)
        {
            var objDMCol = await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_TypeID",
                AddParameters(new SqlParameter("@TypeID", TypeID)));
            return objDMCol.Values.FirstOrDefault();
        }

        public async Task<List<InvestigationCategory>>
            Investigation_Select_Categories(string Sort) =>
            await GetListData<InvestigationCategory>("Investigation_Select_Categories",
                AddParameters(new SqlParameter("@Sort", Sort)));

        public async Task<InvestigationCategory>
            Investigation_Select_Categories_by_Category(string Category)
        {
            var objdmlcol = await GetListData<InvestigationCategory>(
                "Investigation_Select_Categories_by_Category",
                AddParameters(new SqlParameter("@Category", Category)));
            return objdmlcol.FirstOrDefault();
        }

        public async Task<Dictionary<long, PatientsInvestigations>>
            Investigation_Select_Invesigation_by_Admission(string Sort, long Admession) =>
            await GetDictionaryData<PatientsInvestigations>("Investigation_Select_Invesigation_by_Admission",
                AddParameters(new SqlParameter[] {
                    new SqlParameter("@Sort", Sort) ,
                    new SqlParameter("@Admession", Admession)}));

        public async Task<PatientInvestigation>
            Investigation_Select_Invesigation_by_PIID(string PIID)
        {
            var objDMCol = await GetDictionaryData<PatientInvestigation>("Investigation_Select_Invesigation_by_PIID",
                AddParameters(new SqlParameter("@PIID", PIID)));
            return objDMCol.Values.FirstOrDefault();
        }

        public async Task<Dictionary<string, InvestigationFile>>
            Investigation_Select_InvesigationFiles_ID_by_PIID(string PIID) =>
            await SetDataInFileTableInDMCol
            ("Investigation_Select_InvesigationFiles_ID_by_PIID",
                AddParameters(new SqlParameter("@PIID", PIID)));

        public async Task<InvestigationFile>
            Investigation_Select_InvesigationFiles_by_ID(string FileID)
        {
            var objdmlcol = await SetDataInFileTableInDMCol
                ("Investigation_Select_InvesigationFiles_by_ID",
                AddParameters(new SqlParameter("@FileID", FileID)));
            return objdmlcol.Values.FirstOrDefault();
        }
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Doctor_Add_PatientInvestigation(PatientInvestigation PatientInvestigation)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Doctor_Add_PatientInvestigation", AddParameters(new SqlParameter[] {
                new SqlParameter("@PIID", PatientInvestigation.PIID),
                new SqlParameter("@AdmessionID", PatientInvestigation.AdmessionID),
                new SqlParameter("@Type", PatientInvestigation.Type),
                new SqlParameter("@Finish", PatientInvestigation.Finish),
                new SqlParameter("@Note", PatientInvestigation.Note),
                new SqlParameter("@DoctorID", PatientInvestigation.DoctorID),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, PatientInvestigation.PIID);
        }

        public async Task<StoredResult> Investigation_Edit_PatientInvestigation(PatientInvestigation PatientInvestigation)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Edit_PatientInvestigation", AddParameters(
                new SqlParameter[]
            {
                new SqlParameter("@PIID", PatientInvestigation.PIID),
                new SqlParameter("@Result", PatientInvestigation.Result),
                new SqlParameter("@Comment", PatientInvestigation.Comment),
                new SqlParameter("@StaffID", PatientInvestigation.StaffID),
                new SqlParameter("@Finish", PatientInvestigation.Finish),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, PatientInvestigation.PIID);
        }

        public async Task<StoredResult> Investigation_Add_InvestigationFile(InvestigationFile InvestigationFile)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Add_InvestigationFile", AddParameters(
                SetParameter(InvestigationFile, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, InvestigationFile.FileID);
        }

        public async Task<StoredResult> Investigation_Edit_InvestigationFile(InvestigationFile InvestigationFile)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Edit_InvestigationFile", AddParameters(
                SetParameter(InvestigationFile, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, InvestigationFile.FileID);
        }

        public async Task<StoredResult> Investigation_Add_Category(InvestigationCategory InvestigationCategory)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Investigation_Add_Category", AddParameters(
                SetParameter(InvestigationCategory, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, InvestigationCategory.CategoryID);
        }

        public async Task<StoredResult> Investigation_Edit_Category(InvestigationCategory InvestigationCategory)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Edit_Category", AddParameters(
                SetParameter(InvestigationCategory, outputparameter)));
            string id = outputparameter.Value.ToString();
            return await Check(id, InvestigationCategory.CategoryID);
        }

        public async Task<StoredResult> Investigation_Add_Invesigation(Investigation Investigation)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Investigation_Add_Invesigation", AddParameters(new SqlParameter[] {
                new SqlParameter("@TypeID", Investigation.TypeID),
                new SqlParameter("@Type", Investigation.Type),
                new SqlParameter("@CategoryID", Investigation.CategoryID),
                new SqlParameter("@Category", Investigation.Category),
                new SqlParameter("@Sort", Investigation.Sort),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Investigation.TypeID);
        }

        public async Task<StoredResult> Investigation_Edit_Invesigation(Investigation Investigation)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Edit_Invesigation", AddParameters(new SqlParameter[] {
                new SqlParameter("@TypeID", Investigation.TypeID),
                new SqlParameter("@Type", Investigation.Type),
                new SqlParameter("@CategoryID", Investigation.CategoryID),
                new SqlParameter("@Sort", Investigation.Sort),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Investigation.TypeID);
        }

        public async Task<StoredResult> Investigation_Get_Sample(PatientInvestigation Investigation)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Get_Sample", AddParameters(new SqlParameter[] {
                new SqlParameter("@PIID", Investigation.PIID),
                new SqlParameter("@Finish", Investigation.Finish),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, Investigation.PIID);
        }

        public async Task<StoredResult> Investigation_Set_BloodGroup(string BloodGroup, string PatientSSN)
        {
            SqlParameter outputparameter = Outputparameter;
            await ExecuteQuery("Investigation_Set_BloodGroup", AddParameters(new SqlParameter[] {
                new SqlParameter("@SSN", PatientSSN),
                new SqlParameter("@BloodGroup", BloodGroup),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, PatientSSN);
        }
        #endregion

        #region Private Methods
        SqlParameter[] SetParameter(InvestigationFile InvestigationFile, SqlParameter parameter) =>
            new SqlParameter[] {
                new SqlParameter("@FileID", InvestigationFile.FileID),
                new SqlParameter("@FileData", InvestigationFile.FileData),
                new SqlParameter("@FileExtention", InvestigationFile.FileExtention),
                new SqlParameter("@PID", InvestigationFile.PID),
                parameter
            };

        SqlParameter[] SetParameter(InvestigationCategory _Category, SqlParameter parameter) =>
           new SqlParameter[] {
                new SqlParameter("@Category", _Category.Category),
                new SqlParameter("@Sort", _Category.Sort),
                new SqlParameter("@CategoryID", _Category.CategoryID),
                parameter
           };

        async Task<Dictionary<string, InvestigationFile>>
            SetDataInFileTableInDMCol(string StoredProcedure, SqlCommand cmd)
        {
            var files = await GetListData<InvestigationFile>(StoredProcedure, cmd);
            return files.ToDictionary(key => key.FileID);
        }
        #endregion
    }
}