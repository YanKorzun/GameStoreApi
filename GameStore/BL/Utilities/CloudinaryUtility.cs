using CloudinaryDotNet;
using GameStore.BL.Interfaces;
using GameStore.WEB.Settings;

namespace GameStore.BL.Utilities
{
    public class CloudinaryUtility : ICloudinaryUtility
    {
        private readonly Account _account;
        private readonly Cloudinary _cloudinary;

        public CloudinaryUtility(AppSettings appSettings)
        {
            _account = new Account(
                appSettings.CloudinarySettings.Name,
                appSettings.CloudinarySettings.APIkey,
                appSettings.CloudinarySettings.APISecret
                );
            _cloudinary = new Cloudinary(_account);
        }
    }
}