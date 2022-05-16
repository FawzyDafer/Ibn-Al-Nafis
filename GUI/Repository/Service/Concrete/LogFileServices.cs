using GUI.Models.Entities;
using M3Y.Entities;
using M3Y.Repository;
using System;
using System.Threading.Tasks;

namespace GUI.Repository.Service.Concrete
{
    public class LogFileServices : ILogFileServices
    {
        #region Private Variables
        readonly ILogFileRepository LogFileRepository;
        #endregion

        #region Constructors
        public LogFileServices(
            ILogFileRepository logFileRepository)
        {
            LogFileRepository = logFileRepository;
        }
        #endregion

        #region Get Methods

        #endregion

        #region Operation Methods
        public async Task<StoredResult> AddLogFile(LogFile logFile)
        {
            logFile.LogFileID = await Generics.GetIDAsyc();
            logFile.Datetime = DateTime.Now;
            return await LogFileRepository.LogFile_Add(logFile);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
