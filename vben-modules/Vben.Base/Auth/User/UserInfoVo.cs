namespace Vben.Base.Auth.User;

public class UserInfoVo
{
    /**
    * 用户基本信息
    */
    public SysUserVo user { get; set; }

    /**
     * 菜单权限
     */
    public HashSet<String> permissions { get; set; }

    /**
     * 角色权限
     */
    public HashSet<String> roles { get; set; }
}