using Authontication.Core.Entities.Identity;

using Authontication.Core.intrtfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authontication.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;

        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            // Private claims (User-defined)
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"], // Ensure this matches the configuration key
                expires: DateTime.UtcNow.AddSeconds(double.Parse(_configuration["JWT:DurationInSecond"].ToString())), // Ensure the value is parsed correctly
                claims: authClaims,
              
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.Aes128CbcHmacSha256)
     

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = TokenStore.GetToken(refreshToken);

            if (token == null || token.IsRevoked || token.Expiration <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            TokenStore.RevokeToken(refreshToken);
        }



    }
}
