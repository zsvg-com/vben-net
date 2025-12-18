namespace Vben.Common.Sqlsugar.Config;

public class DbConfig
{
    public bool ShowSql { get; set; }= true;
    
    public List<ConnectionConfig> ConnectionConfigs { get; set; }
}