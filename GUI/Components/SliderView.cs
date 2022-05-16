using GUI.Repository.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Components
{
    public class SliderView : ViewComponent
    {
        #region Private Variables
        readonly IImageService ImageService;
        #endregion

        public SliderView(IImageService imageService)
        {
            ImageService = imageService;
        }

        public async Task<IViewComponentResult> InvokeAsync() =>
            View(await ImageService.GetImageByCategory("Slider"));

    }
}
