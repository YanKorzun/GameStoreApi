using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameStore.BL.Interfaces;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.Constants;

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
                appSettings.CloudinarySettings.APIkey,
                appSettings.CloudinarySettings.APISecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ServiceResult<ImageUploadResult>> Upload(IFormFile file)
        {
            ImageUploadResult uploadResult = null;
            if (!CheckFileExtension(file))
            {
                return new(ServiceResultType.InvalidData, FileExtensionException);
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new(ServiceResultType.Success, uploadResult);
        }

        private static bool CheckFileExtension(IFormFile file) => Regex.IsMatch(file.FileName, RegexConstants.FileExtensionRegex);
    }
}