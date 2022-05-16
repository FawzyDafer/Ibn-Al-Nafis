using GUI.Models;
using M3Y.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository.Service
{
    public interface IImageService
    {
        #region Get Methods
        Task<List<Images>> GetImages();
        Task<Images> GetImageById(string ImageId);
        Task<List<Images>> GetImageByCategory(string Category);
        #endregion

        #region Operation Methods
        Task<StoredResult> AddImage(Images images);

        Task<StoredResult> EditImage(Images images);
        #endregion
    }
}