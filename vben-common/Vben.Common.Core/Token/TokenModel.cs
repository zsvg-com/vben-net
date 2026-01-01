namespace Vben.Common.Core.Token;

public class TokenModel
{
    /// <summary>
    /// 用户id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 部门id
    /// </summary>
    public string DeptId { get; set; }

    /// <summary>
    /// 登录用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 角色集合(eg：admin,common)
    /// </summary>
    public List<string> RoleKeys { get; set; } = [];

    /// <summary>
    /// 角色集合(数据权限过滤使用)
    /// </summary>
    public List<Roles> Roles { get; set; }

    /// <summary>
    /// Jwt过期时间
    /// </summary>
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public string TenantId { get; set; }

    /// <summary>
    /// 用户所有权限
    /// </summary>
    public List<string> Permissions { get; set; } = [];

    public TokenModel()
    {
    }

    public TokenModel(TokenModel info, List<Roles> roles)
    {
        UserId = info.UserId;
        UserName = info.UserName;
        DeptId = info.DeptId;
        Roles = roles;
        NickName = info.NickName;
        RoleKeys = roles.Select(f => f.RoleKey).ToList();
    }

    public bool HasPermission(string permission)
    {
        if (IsAdmin()) return true;
        return Permissions != null && Permissions.Contains(permission);
    }

    /// <summary>
    /// 是否管理员
    /// </summary>
    /// <returns></returns>
    public bool IsAdmin()
    {
        return RoleKeys.Contains("admin") || UserId == "u1";
    }
}

public class Roles
{
    public long RoleId { get; set; }
    public string RoleKey { get; set; }
    public int DataScope { get; set; }
}