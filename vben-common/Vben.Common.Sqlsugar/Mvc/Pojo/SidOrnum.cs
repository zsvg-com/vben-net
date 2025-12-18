namespace Vben.Common.Sqlsugar.Mvc.Pojo;

public class SidOrnum
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public string id { get; set; }

    public int ornum { get; set; }
}