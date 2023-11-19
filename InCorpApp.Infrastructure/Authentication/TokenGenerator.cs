using InCorpApp.Application.Abstractions.Authentication;
using InCorpApp.Contracts.Authentication;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Infrastructure.Authentication
{
    public class TokenGenerator: ITokenGenerator
    {
        private readonly JwtSettings _settings;

        public TokenGenerator(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateAccessToken(User user)
        {
            var signInCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim("Email", user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim("Role", user.Role.ToString()),
            };

            var securityToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: new DateTimeProvider().CurrentDateTime().AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: signInCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public string GenerateRefreshToken()
        {
            var signInCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            List<Claim> claims = new();


            var securityToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: new DateTimeProvider().CurrentDateTime().AddHours(2),
            signingCredentials: signInCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public bool ValidateRefreshoken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,

            };

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
            return claimsPrincipal.Identity.IsAuthenticated;
        }

        public ClaimsPrincipal ValidateAccessTokenWithoutLifetime(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,

            };

            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, validationParameters, out _);

            return claimsPrincipal;
        }

    }
}
