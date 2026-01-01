using Microsoft.Extensions.Configuration;

namespace Vben.Common.Core.Token;

public static class JwtSettings
{
    /// <summary>
    /// token是谁颁发的
    /// </summary>
    public static  string? Issuer { get; set; }

    /// <summary>
    /// token可以给那些客户端使用
    /// </summary>
    public static  string? Audience { get; set; }

    /// <summary>
    /// 加密的key（SecretKey必须大于16个,是大于，不是大于等于）
    /// </summary>
    public static  string? SecretKey { get; set; }

    /// <summary>
    /// token时间（分）
    /// </summary>
    public static  int Expire { get; set; } = 1440;

    /// <summary>
    /// 刷新token时长
    /// </summary>
    public static  int RefreshTokenTime { get; set; }

    /// <summary>
    /// token类型
    /// </summary>
    public static  string? TokenType { get; set; } = "Bearer";
    
    public static void Initialize(IConfiguration configuration)
    {
        var section = configuration.GetSection("JwtSettings");
        Issuer = section.GetValue<string>("Issuer");
        Audience = section.GetValue<string>("Audience");
        SecretKey = section.GetValue<string>("SecretKey");
        Expire = section.GetValue<int>("Expire");
        RefreshTokenTime = section.GetValue<int>("RefreshTokenTime");
        TokenType = section.GetValue<string>("TokenType");
    }
    
    
}