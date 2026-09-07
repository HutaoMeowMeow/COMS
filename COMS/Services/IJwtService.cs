using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using COMS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace COMS.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    Guid? ValidateToken(string token);
}
