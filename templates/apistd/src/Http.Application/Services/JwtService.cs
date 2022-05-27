using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Http.Application.Services;

public class JwtService
{
    /// <summary>
    /// 过期时间，分
    /// </summary>
    public int TokenExpires { get; set; }
    public int RefreshTokenExpires { get; set; }
    public string Sign { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public List<Claim>? Claims { get; set; }

    public JwtService(string sign, string audience, string issuer)
    {
        Sign = sign;
        Audience = audience;
        Issuer = issuer;
    }

    /// <summary>
    /// 生成jwt token
    /// </summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public string GetToken(string id, string role)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Sign));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
                // 此处自定义claims
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
        };
        if (Claims != null && Claims.Any())
        {
            claims.AddRange(Claims);
        }
        var jwt = new JwtSecurityToken(Issuer, Audience, claims,
            expires: DateTime.UtcNow.AddMinutes(TokenExpires),
            signingCredentials: signingCredentials);
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}
