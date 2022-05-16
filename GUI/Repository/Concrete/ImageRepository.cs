using GUI.Models;
using M3Y.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class ImageRepository : GenericRepo, IImageRepository
    {
        #region Constructors
        public ImageRepository(IConfiguration Configuration) : base(Configuration) { }
        #endregion

        #region GetMethods
        public async Task<List<Images>> Image_Select_All() =>
            await GetListData<Images>("Image_Select_All", CMD);

        public async Task<Images> Image_Select_By_ImageId(string ImageId)
        {
            var images = await GetListData<Images>("Image_Select_By_ImageId",
                AddParameters(new SqlParameter("@ImageId", ImageId)));
            return images.FirstOrDefault();
        }

        public async Task<List<Images>> Image_Select_By_Category(string Category)
            => await GetListData<Images>("Image_Select_By_Category",
                AddParameters(new SqlParameter("@Category", Category)));
        #endregion

        #region OperationMethods
        public async Task<StoredResult> Image_Add(Images Images)
        {
            var outputparameter = Outputparameter;
            await Insert("Image_Add", GetParameters(Images, outputparameter));
            string id = outputparameter.Value.ToString();
            return await Check(id, Images.ImageId);
        }

        public async Task<StoredResult> Image_Edit(Images Images)
        {
            var outputparameter = Outputparameter;
            await ExecuteQuery("Image_Edit", GetParameters(Images, outputparameter));
            string id = outputparameter.Value.ToString();
            return await Check(id, Images.ImageId);
        }
        #endregion

        #region Private Methods
        SqlCommand GetParameters(Images images, SqlParameter parameter) => AddParameters(new SqlParameter[]
            {
                new SqlParameter("@ImageId", images.ImageId),
                new SqlParameter("@ImageData", images.ImageData),
                new SqlParameter("@ImageExtention", images.ImageExtention),
                new SqlParameter("@Category", images.Category),
                new SqlParameter("@Title", images.Title),
                parameter
            });
        #endregion
    }
}
