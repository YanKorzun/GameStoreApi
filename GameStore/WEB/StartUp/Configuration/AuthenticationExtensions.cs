using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameStore.Startup.Configuration
{
    public static class AuthenticationExtensions
    {
        public static void RegisterAuthSettings(this IServiceCollection services, TokenSettings tokenSettings)
        {
            services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = tokenSettings.ValidateIssuer,
                        ValidIssuer = tokenSettings.Issuer,

                        ValidateAudience = tokenSettings.ValidateAudience,
                        ValidAudience = tokenSettings.Audience,

                        ValidateLifetime = tokenSettings.ValidateLifeTime,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.SigningKey)),
                        ValidateIssuerSigningKey = tokenSettings.ValidateIssuerSigningKey
                    };
                });
        }
    }
}