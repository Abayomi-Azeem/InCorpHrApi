using InCorpApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Abstractions.Authentication
{
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();
        public bool ValidateRefreshoken(string refreshToken);
        public ClaimsPrincipal ValidateAccessTokenWithoutLifetime(string accessToken);
    }
}
