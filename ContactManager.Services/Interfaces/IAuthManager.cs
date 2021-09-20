using ContactManager.Model.DTO;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactManager.Services
{
    public interface IAuthManager
    {
        public Task<string> CreateToken();

        public Task<bool> ValidateUser(LoginDTO loginDTO);

        public Task<List<Claim>> GetClaims();

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    }
}