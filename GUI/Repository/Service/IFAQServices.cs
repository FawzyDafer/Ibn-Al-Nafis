using GUI.Models.Entities;
using M3Y.Entities;
using System.Threading.Tasks;

namespace GUI.Repository.Service
{
    public interface IFAQServices
    {
        #region Get Methods

        #endregion

        #region Operation Methods
        Task<StoredResult> FAQ_Add(FAQ FAQ);
        #endregion

    }
}
