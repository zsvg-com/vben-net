namespace Vben.Base.Sys.Menu;

[SugarTable("sys_menu", TableDescription = "权限菜单")]
[Description("权限菜单")]
public class SysMenu
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }
    
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", IsNullable = true, Length = 32)]
    public  string name { get; set; }
    
    [SugarColumn(ColumnDescription = "父ID", IsNullable = true)]
    public long pid { get; set; }
    
    /// <summary>
    /// 是否可用，1可用，0不可用
    /// </summary>
    [SugarColumn(ColumnDescription = "可用标记：1可用，0禁用", IsNullable = true)]
    public virtual bool avtag { get; set; } = true;

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
    [SugarColumn(ColumnDescription = "类型 C目录，M菜单", IsNullable = true, Length = 8)]
    public string type { get; set; }
    
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public string notes { get; set; }

    [SugarColumn(ColumnDescription = "图标", IsNullable = true, Length = 64)]
    public string icon { get; set; }

    [SugarColumn(ColumnDescription = "路由地址", IsNullable = true, Length = 64)]
    public string path { get; set; }
    
    [SugarColumn(ColumnDescription = "路由参数", IsNullable = true, Length = 64)]
    public string param { get; set; }
    
    [SugarColumn(ColumnDescription = "组件路径", IsNullable = true, Length = 64)]
    public string comp { get; set; }
    
    [SugarColumn(ColumnDescription = "是否显示", IsNullable = true)]
    public bool shtag { get; set; }
    
    [SugarColumn(ColumnDescription = "缓存标记", IsNullable = true)]
    public bool catag { get; set; }
    
    [SugarColumn(ColumnDescription = "是否为外链", IsNullable = true)]
    public bool outag { get; set; }
    
    [SugarColumn(IsIgnore = true)]
    public List<SysMenu> children { get; set; } =new ();
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public DateTime? uptim { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(cruid))]
    public SysOrg crman { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnName = "cruid", ColumnDescription = "创建者Id", IsNullable = true, IsOnlyIgnoreUpdate = true,
        Length = 36)]
    public string cruid { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(upuid))]
    public SysOrg upman { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnName = "upuid", ColumnDescription = "修改者Id", IsNullable = true, Length = 36)]
    public string upuid { get; set; }

    public SysMenu(long id)
    {
        this.id = id;
    }
    public SysMenu()
    {
        
    }
    
    /**
   * 是否为菜单内部跳转
   */
    public bool isMenuFrame() {
        return pid == 0L && "2"==type && !outag;
    }
    
    /**
   * 获取路由名称
   */
    public string getRouteName() {
        String routerName = StrUtils.UpperFirst(path);
        // 非外链并且是一级目录（类型为目录）
        if (isMenuFrame()) {
            routerName = StrUtils.EMPTY;
        }
        return routerName;
    }
    
    /**
    * 是否为内链组件
    */
    public bool isInnerLink() {
        return outag && StrUtils.IsUrl(path);
    }

    /**
   * 内链域名特殊字符替换
   */
    public static string innerLinkReplaceEach(string path) {
        return StrUtils.ReplaceEach(path, new []{"http://", "https://", "www.", ".", ":"},
            new []{"", "", "", "/", "/"});
    }

}