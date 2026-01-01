namespace Vben.Base.Sys.Dept;

public class SysDeptVo
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public string id { get; set; }

    public string name { get; set; }
    
    public string type { get; set; }

    public DateTime? crtim { get; set; }

    public DateTime? uptim { get; set; }
    
    public string cruna  { get; set; }

    public string upuna { get; set; }

    public string notes { get; set; }

    [JsonIgnore] public string pid { get; set; }

    [SugarColumn(IsIgnore = true)] public List<SysDeptVo> children { get; set; }


}