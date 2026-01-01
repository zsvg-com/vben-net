namespace Vben.Base.Sys.User.bo;

/// <summary>
/// 用户查询对象
/// </summary>
public class UserSearchBo
{
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 部门ID
    /// </summary>
    public string depid { get; set; }
    
    /// <summary>
    /// 用户账号名称
    /// </summary>
    public string username { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>
    public string email { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    public string monum { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string notes { get; set; }

    /// <summary>
    /// 判断是否除了部门ID其他查询字段都会空
    /// </summary>
    public bool IsAllNullWithoutDepit()
    {
        return new[] { name, username, email, monum, notes }.All(string.IsNullOrWhiteSpace) && !string.IsNullOrWhiteSpace(depid);
    }
    
}