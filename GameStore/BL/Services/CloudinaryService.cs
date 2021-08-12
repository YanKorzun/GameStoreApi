using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.Constants;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;

namespace GameStore.BL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private const string FileExtensionException = "File doesn't match needed extension";

        private readonly Cloudinary _cloudinary;

        public CloudinaryService(AppSettings appSettings)
        {
            var account = new Account(
                appSettings.CloudinarySettings.Name,
                appSettings.CloudinarySettings.ApiKey,
                appSettings.CloudinarySettings.ApiSecret
            );
            _cloudinary = new(account);
        }

        public async Task<ServiceResult<ImageUploadResult>> Upload(IFormFile file)
        {
            if (!CheckFileExtension(file))
            {
                return new(ServiceResultType.InvalidData, FileExtensionException);
            }

            var uploadParams = new ImageUploadParams
            {
                File = new(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new(ServiceResultType.Success, uploadResult);
        }

        private static bool CheckFileExtension(IFormFile file) =>
            Regex.IsMatch(file.FileName, RegexConstants.FileExtensionRegex);
    }
}