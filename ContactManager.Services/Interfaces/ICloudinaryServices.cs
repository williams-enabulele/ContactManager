using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ContactManager.Services
{
    public interface ICloudinaryServices
    {
        public Task<UploadResult> ImageUploadAsync(IFormFile image);
    }
}