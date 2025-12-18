namespace Vben.Base.Sys.Perm.Api;

[SugarTable("sys_perm_api", TableDescription = "权限接口")]
[Description("权限接口")]
public class SysPermApi
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }
    
    [SugarColumn(ColumnDescription = "接口名称", IsNullable = true, Length = 32)]
    public string name { get; set; }

    [SugarColumn(ColumnDescription = "菜单ID", IsNullable = true)]
    public long menid { get; set; }

    [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
    public int ornum { get; set; }
    
    [SugarColumn(ColumnDescription = "权限字符", IsNullable = true, Length = 64)]
    public string perm { get; set; }

    [SugarColumn(ColumnDescription = "权限代码", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public long code { get; set; }

    [SugarColumn(ColumnDescription = "权限位", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public int pos { get; set; }
    
    [SugarColumn(ColumnDescription = "权限类型", IsNullable = true)]
    public string type { get; set; }
    
    [SugarColumn(ColumnDescription = "是否可用", IsNullable = true)]
    public bool avtag { get; set; } = true;
    
    [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 255)]
    public string notes { get; set; }
    
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

    [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
    public DateTime? uptim { get; set; }

    public SysPermApi(long id)
    {
        this.id = id;
    }
    
    public SysPermApi()
    {
    } 
}