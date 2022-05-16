using GUI.Models;
using M3Y.Entities;
using M3Y.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository.Service.Concrete
{
    public class ImageService : IImageService
    {
        #region Private Variables
        readonly IImageRepository ImageRepository;
        #endregion

        #region Constructors
        public ImageService(IImageRepository imageRepository) =>
            ImageRepository = imageRepository;
        #endregion

        #region Get Methods
        public async Task<List<Images>> GetImages() =>
            await ImageRepository.Image_Select_All();

        public async Task<Images> GetImageById(string ImageId) =>
            await ImageRepository.Image_Select_By_ImageId(ImageId);

        public async Task<List<Images>> GetImageByCategory(string Category)
            => await ImageRepository.Image_Select_By_Category(Category);
        #endregion

        #region Operation Methods
        public async Task<StoredResult> AddImage(Images images)
        {
            images.ImageId = await Generics.GetIDAsyc();
            var imagefile = await Generics.GetFileAsync(images.ImageFile);
            images.ImageData = imagefile.Data;
            images.ImageExtention = imagefile.Extention;
            return await ImageRepository.Image_Add(images);
        }

        public async Task<StoredResult> EditImage(Images images)
        {
            if (images.ImageFile != null)
            {
                var imagefile = await Generics.GetFileAsync(images.ImageFile);
                images.ImageData = imagefile.Data;
                images.ImageExtention = imagefile.Extention;
            }
            else
            {
                var image = await GetImageById(images.ImageId);
                images.ImageData = image.ImageData;
                images.ImageExtention = image.ImageExtention;
            }
            return await ImageRepository.Image_Edit(images);
        }

        #endregion
    }
}
