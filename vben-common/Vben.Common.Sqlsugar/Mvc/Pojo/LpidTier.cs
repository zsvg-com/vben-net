namespace Vben.Common.Sqlsugar.Mvc.Pojo;

public class LpidTier
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }

    public string tier { get; set; }
    
    public long pid { get; set; }
}