using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameStore.BL.Interfaces;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
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

        public async Task<ImageUploadResult> Upload(IFormFile file)
        {
            var path = string.Empty;
            if (CheckIfExcelFile(file))
            {
                path = await WriteFile(file);
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path)
            };
            var uploadResult = _cloudinary.Upload(uploadParams);
            return uploadResult;
        }

        private static bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension is ".jpg" or ".jpeg" or ".bmp");
        }

        private static async Task<string> WriteFile(IFormFile file)
        {
            var pathToFile = string.Empty;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                    fileName);

                await using var stream = new FileStream(pathToFile, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            catch (Exception)
            {
            }
            return pathToFile;
        }
    }
}