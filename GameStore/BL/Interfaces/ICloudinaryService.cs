using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;

namespace GameStore.BL.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ServiceResult<ImageUploadResult>> Upload(IFormFile file);
    }
}