using GUI.Models.Entities;
using M3Y.Entities;
using System.Threading.Tasks;

namespace GUI.Repository.Service
{
    public interface ILogFileServices
    {
        #region Get Methods

        #endregion

        #region Operation Methods
        Task<StoredResult> AddLogFile(LogFile logFile);
        #endregion
    }
}
