namespace Vben.Common.Core.Const;

public class CommonConst
{
    /// <summary>
    /// 用户缓存
    /// </summary>
    public const string CACHE_KEY_USER = "user_";

    /// <summary>
    /// 菜单缓存
    /// </summary>
    public const string CACHE_KEY_MENU = "menu_";

    /// <summary>
    /// 权限缓存
    /// </summary>
    public const string CACHE_KEY_PERMISSION = "permission_";

    /// <summary>
    /// 数据范围缓存
    /// </summary>
    public const string CACHE_KEY_DATASCOPE = "datascope_";

    /// <summary>
    /// 验证码缓存
    /// </summary>
    public const string CACHE_KEY_CODE = "vercode_";


    public static string[] ENTITY_ASSEMBLY_NAME = new string[] { "Vben.Common.Core", "Vben.App" };

    /// <summary>
    /// 库表实体信息缓存
    /// </summary>
    public const string CACHE_KEY_ENTITYINFO = "tableentity";

    /// <summary>
    /// 所有权限缓存
    /// </summary>
    public const string CACHE_KEY_ALLPERMISSION = "allpermission";
}