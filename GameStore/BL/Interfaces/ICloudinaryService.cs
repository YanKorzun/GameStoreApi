using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using GameStore.BL.ResultWrappers;
using Microsoft.AspNetCore.Http;

namespace GameStore.BL.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ServiceResult<ImageUploadResult>> Upload(IFormFile file);
    }
}