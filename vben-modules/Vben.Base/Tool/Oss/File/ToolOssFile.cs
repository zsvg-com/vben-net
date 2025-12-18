namespace Vben.Base.Tool.Oss.File;

[SugarTable("tool_oss_file", TableDescription = "OSS存储文件")]
public class ToolOssFile
{
    [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, Length = 32)]
    public string id { get; set; }

    [SugarColumn(ColumnDescription = "文件md5", IsNullable = true, Length = 32)]
    public string md5 { get; set; }

    [SugarColumn(ColumnDescription = "文件大小", IsNullable = true)]
    public long zsize { get; set; }

    [SugarColumn(ColumnDescription = "存储地址", IsNullable = true, Length = 255)]
    public string path { get; set; }

    [SugarColumn(ColumnDescription = "存储服务", IsNullable = true, Length = 32)]
    public string service { get; set; }

    [SugarColumn(ColumnDescription = "创建时间", IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public DateTime? crtim { get; set; } = DateTime.Now;

}