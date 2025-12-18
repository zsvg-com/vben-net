namespace Vben.Base.Auth.User;

public class SysUserVo
{
    /**
    * 用户ID
    */
    public string userId { get; set; }

    /**
     * 租户ID
     */
    public string tenantId { get; set; }

    /**
     * 部门ID
     */
    public string deptId { get; set; }

    /**
     * 用户账号
     */
    public string userName { get; set; }

    /**
     * 用户昵称
     */
    public string nickName { get; set; }

    /**
     * 用户类型（sys_user系统用户）
     */
    public string userType { get; set; }

    /**
     * 用户邮箱
     */
    public string email { get; set; }

    /**
     * 手机号码
     */
    public string phonenumber { get; set; }

    /**
     * 用户性别（0男 1女 2未知）
     */
    public string sex { get; set; }

    /**
     * 头像地址
     */
    public string avatar { get; set; }

    /**
     * 密码
     */
    public string password { get; set; }

    /**
     * 帐号状态（0正常 1停用）
     */
    public string status { get; set; }

    /**
     * 最后登录IP
     */
    public string loginIp { get; set; }

    /**
     * 最后登录时间
     */
    public DateTime loginDate { get; set; }= DateTime.Now;

    /**
     * 备注
     */
    public string remark { get; set; }

    /**
     * 创建时间
     */
    public DateTime createTime { get; set; }= DateTime.Now;

    /**
     * 部门名
     */
    public string deptName { get; set; }

    /**
     * 角色对象
     */
    /**
     * 角色组
     */
    public long[] roleIds { get; set; }

    /**
     * 岗位组
     */
    public long[] postIds { get; set; }

    /**
     * 数据权限 当前角色ID
     */
    public long roleId { get; set; }
}