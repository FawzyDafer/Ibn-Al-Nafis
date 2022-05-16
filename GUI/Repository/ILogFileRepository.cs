using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface ILogFileRepository
    {
        #region Get Methods
        Task<Dictionary<long, UsersLogFiles>> Select_All();
        Task<Dictionary<long, UsersLogFiles>> Select_By_DateTime(DateTime dateTime);
        Task<Dictionary<long, UsersLogFiles>> Select_By_Description(string description);
        Task<Dictionary<long, UsersLogFiles>> Select_By_Job(string job);
        Task<UsersLogFiles> Select_By_LogFileID(string logfileId);
        Task<Dictionary<long, UsersLogFiles>> Select_By_Phone(string phone);
        Task<Dictionary<long, UsersLogFiles>> Select_By_Qualification(string qualification);
        Task<Dictionary<long, UsersLogFiles>> Select_By_RoleID(string roleId);
        Task<Dictionary<long, UsersLogFiles>> Select_By_Specialization(string specialization);
        Task<Dictionary<long, UsersLogFiles>> Select_By_UserID(string userId);
        Task<Dictionary<long, UsersLogFiles>> Select_By_UserName(string username);
        #endregion

        #region Operation Methods
        Task<StoredResult> LogFile_Add(LogFile logFile);
        #endregion

    }
}
