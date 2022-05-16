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
    public class FAQRepository : GenericRepo, IFAQRepository
    {
        #region Constructors
        public FAQRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<Dictionary<string, FAQ>> FAQ_select_by_Question(string Question) =>
            await SetDataTableInDMCol("FAQ_select_by_Question",
                AddParameters(new SqlParameter("@Question", Question)));

        public async Task<Dictionary<string, FAQ>> FAQ_select_by_UserID(string UserID)
            => await SetDataTableInDMCol("FAQ_select_by_UserID",
                AddParameters(new SqlParameter("@UserID", UserID)));

        public async Task<Dictionary<string, FAQ>> FAQ_select_All() =>
            await SetDataTableInDMCol("FAQ_select_All", CMD);

        public async Task<Dictionary<string, FAQ>> FAQ_Select_by_date(DateTime Date) =>
            await SetDataTableInDMCol("FAQ_Select_by_date",
                AddParameters(new SqlParameter("@Date", Date)));

        public async Task<FAQ> FAQ_Select_by_ID(string QustionID)
        {
            var objDMCol = await SetDataTableInDMCol("FAQ_Select_by_ID",
                AddParameters(new SqlParameter("@QustionID", QustionID)));
            return objDMCol.Values.FirstOrDefault();
        }
        #endregion

        #region OperationMethods
        public async Task<StoredResult> FAQ_Add(FAQ FAQ)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("FAQ_Add", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@QuestionID", FAQ.QuestionID),
                new SqlParameter("@Question", FAQ.Question),
                new SqlParameter("@UserID", FAQ.UserID),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, FAQ.QuestionID);
        }
        #endregion

        #region Private Methods
        async Task<Dictionary<string, FAQ>> SetDataTableInDMCol
            (string StoredProcedure, SqlCommand cmd)
        {
            var fAQs = await GetListData<FAQ>(StoredProcedure, cmd);
            return fAQs.ToDictionary(Key => Key.QuestionID);
        }
        #endregion
    }
}
