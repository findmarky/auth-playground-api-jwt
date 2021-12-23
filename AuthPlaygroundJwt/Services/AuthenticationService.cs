using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Playground.API.Data;
using Auth.Playground.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Playground.API.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IUserAccessStore _userAccessStore;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly JwtTokenSettings _jwtTokenSettings;

        public AuthenticationService(
            IUserAccessStore userAccessStore,
            ILogger<AuthenticationService> logger,
            IOptions<JwtTokenSettings> jwtTokenOptions)
        {
            _userAccessStore = userAccessStore;
            _logger = logger;
            _jwtTokenSettings = jwtTokenOptions?.Value ?? throw new ArgumentNullException(nameof(jwtTokenOptions));
        }

        public AuthenticationResultModel Authenticate(string userName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {
                    return new AuthenticationResultModel { IsAuthenticated = false };
                }

                if (!_userAccessStore.Authenticate(userName, password))
                {
                    return new AuthenticationResultModel { IsAuthenticated = false };
                }

                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.SecretKey));
                SigningCredentials signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenOptions = new JwtSecurityToken(
                    issuer: _jwtTokenSettings.Issuer,
                    audience: _jwtTokenSettings.Audience,
                    claims: new List<Claim>(),
                    expires: DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpiryInMinutes),
                    signingCredentials: signingCredentials
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return new AuthenticationResultModel { IsAuthenticated = true, Token = tokenString };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to authenticate a user");
                return new AuthenticationResultModel { IsAuthenticated = false };
            }
        }
    }
}
