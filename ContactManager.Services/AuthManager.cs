using ContactManager.Model;
using ContactManager.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _currentUser;

        public AuthManager(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates Token
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// Get Token Options
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        /// <summary>
        /// Get Claims
        /// </summary>
        /// <returns></returns>
        public async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, _currentUser.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_currentUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        /// <summary>
        /// Get Signing Credentials
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GetSigningCredentials()
        {
            var jwtsettings = _configuration.GetSection("Jwt");
            var key = jwtsettings.GetSection("key").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Validates User
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUser(LoginDTO loginDTO)
        {
            _currentUser = await _userManager.FindByNameAsync(loginDTO.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_currentUser, loginDTO.Password);
            return (_currentUser != null && validPassword);
        }
    }
}