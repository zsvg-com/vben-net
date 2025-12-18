// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace Admin.NET.Core;

/// <summary>
/// 缓存相关常量
/// </summary>
public class CacheConst
{
    /// <summary>
    /// 用户权限缓存（按钮集合）
    /// </summary>
    public const string KeyUserButton = "sys_user_button:";

    /// <summary>
    /// 用户机构缓存
    /// </summary>
    public const string KeyUserOrg = "sys_user_org:";

    /// <summary>
    /// 角色最大数据范围缓存
    /// </summary>
    public const string KeyRoleMaxDataScope = "sys_role_maxDataScope:";

    /// <summary>
    /// 在线用户缓存
    /// </summary>
    public const string KeyUserOnline = "sys_user_online:";

    /// <summary>
    /// 图形验证码缓存
    /// </summary>
    public const string KeyVerCode = "sys_verCode:";

    /// <summary>
    /// 手机验证码缓存
    /// </summary>
    public const string KeyPhoneVerCode = "sys_phoneVerCode:";

    /// <summary>
    /// 密码错误次数缓存
    /// </summary>
    public const string KeyPasswordErrorTimes = "sys_password_error_times:";

    /// <summary>
    /// 租户缓存
    /// </summary>
    public const string KeyTenant = "sys_tenant";

    /// <summary>
    /// 常量下拉框
    /// </summary>
    public const string KeyConst = "sys_const:";

    /// <summary>
    /// 所有缓存关键字集合
    /// </summary>
    public const string KeyAll = "sys_keys";

    /// <summary>
    /// SqlSugar二级缓存
    /// </summary>
    public const string SqlSugar = "sys_sqlSugar:";

    /// <summary>
    /// 开放接口身份缓存
    /// </summary>
    public const string KeyOpenAccess = "sys_open_access:";

    /// <summary>
    /// 开放接口身份随机数缓存
    /// </summary>
    public const string KeyOpenAccessNonce = "sys_open_access_nonce:";

    /// <summary>
    /// 登录黑名单
    /// </summary>
    public const string KeyBlacklist = "sys_blacklist:";

    /// <summary>
    /// 系统配置缓存
    /// </summary>
    public const string KeyConfig = "sys_config:";

    /// <summary>
    /// 系统租户配置缓存
    /// </summary>
    public const string KeyTenantConfig = "sys_tenant_config:";

    /// <summary>
    /// 系统用户配置缓存
    /// </summary>
    public const string KeyUserConfig = "sys_user_config:";

    /// <summary>
    /// 系统字典缓存
    /// </summary>
    public const string KeyDict = "sys_dict:";

    /// <summary>
    /// 系统租户字典缓存
    /// </summary>
    public const string KeyTenantDict = "sys_tenant_dict:";

    /// <summary>
    /// 重复请求(幂等)字典缓存
    /// </summary>
    public const string KeyIdempotent = "sys_idempotent:";

    /// <summary>
    /// Excel临时文件缓存
    /// </summary>
    public const string KeyExcelTemp = "sys_excel_temp:";

    /// <summary>
    /// 系统更新命令日志缓存
    /// </summary>
    public const string KeySysUpdateLog = "sys_update_log";

    /// <summary>
    /// 系统更新间隔标记缓存
    /// </summary>
    public const string KeySysUpdateInterval = "sys_update_interval";
}