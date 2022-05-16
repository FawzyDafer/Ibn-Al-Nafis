using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Models.Views;
using M3Y.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class LogFileRepository : GenericRepo, ILogFileRepository
    {
        #region Constructors
        readonly UserManager<User> _userManager;
        public LogFileRepository(IConfiguration Configuration,
            UserManager<User> userManager) : base(Configuration)
        {
            _userManager = userManager;
        }
        #endregion

        #region Get Methods
        public async Task<Dictionary<long, UsersLogFiles>> Select_All() =>
            await Getwithrole("Logfile_Select_ALL", CMD);

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_DateTime(DateTime dateTime) =>
            await Getwithrole("Logfile_Select_By_DateTime",
                AddParameters(new SqlParameter("@DateTime", dateTime)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_Description(string description) =>
            await Getwithrole("Logfile_Select_By_Description",
                AddParameters(new SqlParameter("@Description", description)));

        public async Task<Dictionary<long, UsersLogFiles>> Select_By_Job(string job)
            => await Getwithrole("Logfile_Select_By_Jop",
                AddParameters(new SqlParameter("@Jop", job)));

        public async Task<UsersLogFiles> Select_By_LogFileID(string logfileId)
        {
            var objDMCol = await Getwithrole(
                "Logfile_Select_By_LogFileID",
                AddParameters(new SqlParameter("@LogFileID", logfileId)));
            return objDMCol.Values.LastOrDefault();
        }

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_Phone(string phone) =>
            await Getwithrole("Logfile_Select_By_PhoneNumber",
                AddParameters(new SqlParameter("@PhoneNumber", phone)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_Qualification(string qualification) =>
            await Getwithrole("Logfile_Select_By_Qualification",
                AddParameters(new SqlParameter("@Qualification", qualification)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_RoleID(string roleId) =>
            await Getwithrole("Logfile_Select_By_RoleID",
                AddParameters(new SqlParameter("@RoleID", roleId)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_Specialization(string specialization) =>
            await Getwithrole("Logfile_Select_By_Specialization",
                AddParameters(new SqlParameter("@Specialization", specialization)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_UserID(string userId) =>
            await Getwithrole("Logfile_Select_By_UserId",
                AddParameters(new SqlParameter("@UserID", userId)));

        public async Task<Dictionary<long, UsersLogFiles>>
            Select_By_UserName(string username) =>
            await Getwithrole("Logfile_Select_By_UserName",
                AddParameters(new SqlParameter("@UserName", username)));
        #endregion

        #region Operation Methods 
        public async Task<StoredResult> LogFile_Add(LogFile logFile)
        {
            SqlParameter outputparameter = Outputparameter;
            await Insert("Logfile_Add", AddParameters(new SqlParameter[]
            {
                new SqlParameter("@LogFileId", logFile.LogFileID),
                new SqlParameter("@Description", logFile.Description),
                new SqlParameter("@DateTime", logFile.Datetime),
                new SqlParameter("@UserID", logFile.UserID),
                outputparameter
            }));
            string id = outputparameter.Value.ToString();
            return await Check(id, logFile.LogFileID);
        }
        #endregion

        #region PrivateMethods
        async Task<Dictionary<long, UsersLogFiles>>
            Getwithrole(string Stored, SqlCommand cmd)
        {
            var logfiles = await GetDictionaryData<UsersLogFiles>("Logfile_Select_ALL", CMD);
            foreach (var item in logfiles)
            {
                var user = await _userManager.FindByIdAsync(item.Value.Id);
                var roles = (await _userManager.GetRolesAsync(user)).LastOrDefault();
                item.Value.Jop = roles;
            }
            return logfiles;
        }
        #endregion
    }
}
