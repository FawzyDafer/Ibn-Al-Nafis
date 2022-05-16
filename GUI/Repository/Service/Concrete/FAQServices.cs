using GUI.Models.Entities;
using M3Y.Entities;
using M3Y.Repository;
using System.Threading.Tasks;

namespace GUI.Repository.Service.Concrete
{
    public class FAQServices : IFAQServices
    {
        #region Vaiables
        readonly IFAQRepository FAQRepository;
        #endregion

        #region Constructors
        public FAQServices(
            IFAQRepository fAQRepository)
        {
            FAQRepository = fAQRepository;
        }
        #endregion

        #region Get Methods

        #endregion

        #region Operation Methods
        public async Task<StoredResult> FAQ_Add(FAQ FAQ)
        {
            FAQ.QuestionID = await Generics.GetIDAsyc();
            return await FAQRepository.FAQ_Add(FAQ);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
