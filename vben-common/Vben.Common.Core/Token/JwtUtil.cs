using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Vben.Common.Core.Wrapper;

namespace Vben.Common.Core.Token;

/// <summary>
/// 2023-8-29已从WebApi移至此
/// </summary>
public class JwtUtil
{
    
    /// <summary>
    /// 获取用户身份信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static TokenModel GetLoginUser(HttpContext httpContext)
    {
        string token = httpContext.GetToken();

        if (string.IsNullOrEmpty(token)) return null;

        var tokenModel = ValidateJwtToken(ParseToken(token));
        return tokenModel;
    }

    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(List<Claim> claims)
    {

        var authTime = DateTime.Now;
        var expiresAt = authTime.AddMinutes(JwtSettings.Expire);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtSettings.SecretKey);
        claims.Add(new Claim("Audience", JwtSettings.Audience));
        claims.Add(new Claim("Issuer", JwtSettings.Issuer));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = JwtSettings.Issuer,
            Audience = JwtSettings.Audience,
            IssuedAt = authTime, //token生成时间
            Expires = expiresAt,
            //NotBefore = authTime,
            TokenType = JwtSettings.TokenType,
            //对称秘钥，签名证书
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// 验证Token
    /// </summary>
    /// <returns></returns>
    public static TokenValidationParameters ValidParameters()
    {

        var key = Encoding.ASCII.GetBytes(JwtSettings.SecretKey);

        var tokenDescriptor = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true, //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
            ClockSkew = TimeSpan.FromSeconds(30)
            //RequireExpirationTime = true,//过期时间
        };
        return tokenDescriptor;
    }

    /// <summary>
    /// 从令牌中获取数据声明
    /// </summary>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public static JwtSecurityToken? ParseToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validateParameter = ValidParameters();
        token = token.Replace("Bearer ", "");
        try
        {
            tokenHandler.ValidateToken(token, validateParameter, out SecurityToken validatedToken);

            return tokenHandler.ReadJwtToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            // return null if validation fails
            return null;
        }
    }

    /// <summary>
    /// jwt token校验
    /// </summary>
    /// <param name="jwtSecurityToken"></param>
    /// <returns></returns>
    public static TokenModel? ValidateJwtToken(JwtSecurityToken jwtSecurityToken)
    {
        try
        {
            if (jwtSecurityToken == null) return null;
            IEnumerable<Claim> claims = jwtSecurityToken?.Claims;
            TokenModel loginUser = null;

            var userData = claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            if (userData != null)
            {
                loginUser = JsonConvert.DeserializeObject<TokenModel>(userData);
                loginUser.ExpireTime = jwtSecurityToken.ValidTo;
            }

            return loginUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    ///组装Claims
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static List<Claim> AddClaims(TokenModel user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.PrimarySid, user.UserId.ToString()),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.GroupSid, user.DeptId.ToString()),
            new(ClaimTypes.UserData, JsonConvert.SerializeObject(user)),
        };
        if (user?.TenantId != null)
        {
            //租户ID
            claims.Add(new(ClaimTypes.PrimaryGroupSid, user.TenantId));
        }

        // 只挑选敏感权限
        var sensitivePerms = user.Permissions?.Where(p => p.StartsWith("p:")).ToList();
        if (sensitivePerms != null && sensitivePerms.Count > 0)
        {
            claims.Add(new Claim("sensitivePerms", string.Join(',', sensitivePerms)));
        }

        return claims;
    }
}