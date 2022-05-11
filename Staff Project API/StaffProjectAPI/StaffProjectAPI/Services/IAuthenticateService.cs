using StaffProjectAPI.Data;
using System.IdentityModel.Tokens.Jwt;

namespace StaffProjectAPI.Services
{
    public interface IAuthenticateService
    {
        public JwtSecurityToken GetAuthorizationToken(string userName, IConfiguration configuration);
    }
}
