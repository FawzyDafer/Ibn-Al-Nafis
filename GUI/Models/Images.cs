using GUI.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace GUI.Models
{
    public class Images
    {
        public string ImageId { get; set; }
        [ImageValidation]
        public IFormFile ImageFile { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtention { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
    }
}
