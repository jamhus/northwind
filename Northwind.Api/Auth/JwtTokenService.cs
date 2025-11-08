using Microsoft.IdentityModel.Tokens;
using Northwind.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Northwind.Auth;

public interface IJwtTokenService
{
    string CreateToken(ApplicationUser user, IEnumerable<string> roles);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public string CreateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? user.Email ?? user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        };

        if (user.EmployeeId.HasValue)
            claims.Add(new Claim(CustomClaimTypes.EmployeeId, user.EmployeeId.Value.ToString()));
        if (user.SupplierId.HasValue)
            claims.Add(new Claim(CustomClaimTypes.SupplierId, user.SupplierId.Value.ToString()));

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
