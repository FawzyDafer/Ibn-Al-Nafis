using GUI.Models.Entities;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IFAQRepository
    {
        #region GetMethods
        Task<Dictionary<string, FAQ>> FAQ_select_by_Question(string Question);
        Task<Dictionary<string, FAQ>> FAQ_select_by_UserID(string UserID);
        Task<Dictionary<string, FAQ>> FAQ_select_All();
        Task<Dictionary<string, FAQ>> FAQ_Select_by_date(DateTime Date);
        Task<FAQ> FAQ_Select_by_ID(string QustionID);
        #endregion

        #region OperationMethods
        Task<StoredResult> FAQ_Add(FAQ FAQ);
        #endregion
    }
}
