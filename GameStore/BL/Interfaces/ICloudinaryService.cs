using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> Upload(IFormFile file);
    }
}