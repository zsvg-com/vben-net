using Newtonsoft.Json;

namespace Vben.Common.Sqlsugar.Mvc.Pojo;

//树形pojo
public class Stree
{
    [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
    public string id { get; set; }

    public string name { get; set; }

    public string type { get; set; }

    [JsonIgnore] public string pid { get; set; }

    [SugarColumn(IsIgnore = true)] public List<Stree> children { get; set; }


}