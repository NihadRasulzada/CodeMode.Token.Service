using CodeMode.Token.Service.Configurations;
using CodeMode.Token.Service.Dtos.TokenDto;
using CodeMode.Token.Service.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CodeMode.Token.Service.Services.Concreate
{

    public class TokenService<TUser> : ITokenService<TUser>
        where TUser : IdentityUser
    {
        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        
        public async Task<CreateTokenDto> CreateTokenAsync(TUser user, List<Claim> claims, CustomTokenOptions options)
        {
            DateTime accessTokenExpiration = DateTime.UtcNow.AddHours(4).AddMinutes(options.AccessTokenExpiration);
            DateTime refreshTokenExpiration = DateTime.UtcNow.AddHours(4).AddMinutes(options.RefreshTokenExpiration);

            SecurityKey securityKey = SecurityKeyService.GetSymmetricSecurityKey(options.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: options.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow.AddHours(4),
                claims: claims,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);

            CreateTokenDto tokenDto = new CreateTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }
    }
}
