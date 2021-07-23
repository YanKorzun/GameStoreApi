using GameStore.WEB.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameStore.WEB.Utilities
{
    public class TokenGenerator
    {
        private readonly AppSettings _appSettings;

        public TokenGenerator(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateAccessToken(int userId, string userName, string userRole = "user")
        {
            var claim = GetClaims(userId, userName, userRole);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Token.SigningKey));
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: _appSettings.Token.Issuer,
                    audience: _appSettings.Token.Audience,
                    notBefore: now,
                    claims: claim.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_appSettings.Token.LifeTime)),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static ClaimsIdentity GetClaims(int userId, string userName, string userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, userRole),
                new Claim(ClaimTypes.Name, userName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "Schema"
            );

            return claimsIdentity;
        }
    }
}