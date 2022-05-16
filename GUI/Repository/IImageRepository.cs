using GUI.Models;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IImageRepository
    {
        #region GetMethods
        Task<List<Images>> Image_Select_All();

        Task<Images> Image_Select_By_ImageId(string ImageId);

        Task<List<Images>> Image_Select_By_Category(string Category);
        #endregion

        #region OperationMethods
        Task<StoredResult> Image_Add(Images Images);

        Task<StoredResult> Image_Edit(Images Images);
        #endregion
    }
}
