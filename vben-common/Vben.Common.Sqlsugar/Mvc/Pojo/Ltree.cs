using Newtonsoft.Json;

namespace Vben.Common.Sqlsugar.Mvc.Pojo;

//树形pojo
public class Ltree
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public long id { get; set; }

    public string name { get; set; }

    public string type { get; set; }

    [JsonIgnore] public long pid { get; set; }

    [SugarColumn(IsIgnore = true)] public List<Ltree> children { get; set; }


}