namespace Vben.Base.Sys.User;

[SugarTable("sys_user", TableDescription = "组织架构-用户")]
[Description("组织架构-用户")]
public class SysUser : BaseStrMainEntity
{
    [SugarColumn(ColumnDescription = "部门ID", IsNullable = true, Length = 36)]
    public string depid { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public virtual string depna { get; set; }

    [SugarColumn(ColumnDescription = "层级", IsNullable = true, Length = 512)]
    public string tier { get; set; }

    [SugarColumn(ColumnDescription = "职务", IsNullable = true, Length = 64)]
    public string job { get; set; }

    [SugarColumn(ColumnDescription = "用户名", IsNullable = true, Length = 32)]
    public string username { get; set; }

    // [JsonIgnore]
    [SugarColumn(ColumnDescription = "密码", IsNullable = true, Length = 64, IsOnlyIgnoreUpdate = true)]
    public string password { get; set; }

    [SugarColumn(ColumnDescription = "邮箱", IsNullable = true, Length = 64)]
    public string email { get; set; }

    [SugarColumn(ColumnDescription = "手机号", IsNullable = true, Length = 32)]
    public string monum { get; set; }
    
    
    [SugarColumn(ColumnDescription = "性别", IsNullable = true, Length = 8)]
    public string gender { get; set; }

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }

    [SugarColumn(ColumnDescription = "标签", IsNullable = true, Length = 32)]
    public string label { get; set; }

    [SugarColumn(ColumnDescription = "用户类型", IsNullable = true, Length = 32)]
    public int type { get; set; }

    [SugarColumn(ColumnDescription = "缓存标记", IsNullable = true)]
    public bool catag { get; set; } 
    
    [SugarColumn(ColumnDescription = "最后登录时间", IsNullable = true)]
    public DateTime lotim { get; set; }

    [SugarColumn(ColumnDescription = "最后登录IP", IsNullable = true, Length = 20)]
    public string loip { get; set; }
    
    [SugarColumn(ColumnDescription = "头像", IsNullable = true, Length = 128)]
    public string avatar { get; set; }
    
}