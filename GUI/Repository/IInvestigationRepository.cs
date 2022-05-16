using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IInvestigationRepository
    {
        #region GetMethods
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigations();
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_PatientSSN(string PatientSSN, string Sort);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_Clinic(string Clinic);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_Type(string Type);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_Category(string Category);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_Sort(string Sort);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_InvesigationCategory_by_Category(string Category);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_InvesigationCategory_by_Sort(string Sort);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_InvesigationCategory_by_Type(string Type);
        Task<PatientsInvestigations> Investigation_Select_Invesigation_by_TypeID(string TypeID);
        Task<List<InvestigationCategory>> Investigation_Select_Categories(string Sort);
        Task<InvestigationCategory> Investigation_Select_Categories_by_Category(string Category);
        Task<Dictionary<long, PatientsInvestigations>> Investigation_Select_Invesigation_by_Admission(string Sort, long Admession);
        Task<PatientInvestigation> Investigation_Select_Invesigation_by_PIID(string PIID);
        Task<Dictionary<string, InvestigationFile>> Investigation_Select_InvesigationFiles_ID_by_PIID(string PIID);
        Task<InvestigationFile> Investigation_Select_InvesigationFiles_by_ID(string FileID);
        #endregion

        #region OperationMethods
        Task<StoredResult> Doctor_Add_PatientInvestigation(PatientInvestigation PatientInvestigation);
        Task<StoredResult> Investigation_Edit_PatientInvestigation(PatientInvestigation PatientInvestigation);
        Task<StoredResult> Investigation_Add_InvestigationFile(InvestigationFile InvestigationFile);
        Task<StoredResult> Investigation_Edit_InvestigationFile(InvestigationFile InvestigationFile);
        Task<StoredResult> Investigation_Add_Category(InvestigationCategory InvestigationCategory);
        Task<StoredResult> Investigation_Edit_Category(InvestigationCategory InvestigationCategory);
        Task<StoredResult> Investigation_Add_Invesigation(Investigation Investigation);
        Task<StoredResult> Investigation_Edit_Invesigation(Investigation Investigation);
        Task<StoredResult> Investigation_Get_Sample(PatientInvestigation Investigation);
        Task<StoredResult> Investigation_Set_BloodGroup(string BloodGroup, string PatientSSN);
        #endregion
    }
}
