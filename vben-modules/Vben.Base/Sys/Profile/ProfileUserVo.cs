namespace Vben.Base.Sys.Profile;

public class ProfileUserVo
{
    /**
    * 用户ID
    */
    public string userId {get;set;}

    /**
     * 租户ID
     */
    public string tenantId {get;set;}

    /**
     * 部门ID
     */
    public string deptId  {get;set;}

    /**
     * 用户账号
     */
    public string userName {get;set;}

    /**
     * 用户昵称
     */
    public string nickName {get;set;}

    /**
     * 用户类型（sys_user系统用户）
     */
    public string userType  {get;set;}

    /**
     * 用户邮箱
     */
    public string email  {get;set;}

    /**
     * 手机号码
     */
    public string phonenumber {get;set;}

    /**
     * 用户性别（0男 1女 2未知）
     */
    public string sex {get;set;}

    /**
     * 头像地址
     */
//    @Translation(type = TransConstant.OSS_ID_TO_URL)
    public string avatar {get;set;}

    /**
     * 最后登录IP
     */
    public string loginIp  {get;set;}

    /**
     * 最后登录时间
     */
    public DateTime loginDate {get;set;}

    /**
     * 部门名
     */
//    @Translation(type = TransConstant.DEPT_ID_TO_NAME, mapper = "deptId")
    public string deptName {get;set;}
}