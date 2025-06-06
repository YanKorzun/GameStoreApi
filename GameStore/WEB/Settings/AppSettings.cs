﻿namespace GameStore.WEB.Settings
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public TokenSettings Token { get; set; }
        public SmtpClientSettings SmtpClientSettings { get; set; }
        public CloudinarySettings CloudinarySettings { get; set; }
    }
}