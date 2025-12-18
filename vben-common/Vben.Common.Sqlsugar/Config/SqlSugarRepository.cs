using Vben.Common.Core;

namespace Vben.Common.Sqlsugar.Config;

/// <summary>
/// SqlSugar仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    protected ITenant iTenant = null; // 多租户事务

    public SqlSugarRepository(ISqlSugarClient context = null) : base(context) // 默认值等于null不能少
    {
        iTenant = MyApp.GetService<ISqlSugarClient>().AsTenant();
        base.Context = iTenant.GetConnectionWithAttr<T>();
    }
}