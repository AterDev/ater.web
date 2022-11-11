using System.IdentityModel.Tokens.Jwt;

using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

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
        SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(Sign));
        SigningCredentials signingCredentials = new(signingKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new()
        {
                // 此处自定义claims
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
        };
        if (Claims != null && Claims.Any())
        {
            claims.AddRange(Claims);
        }
        JwtSecurityToken jwt = new(Issuer, Audience, claims,
            expires: DateTime.UtcNow.AddMinutes(TokenExpires),
            signingCredentials: signingCredentials);
        string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}
