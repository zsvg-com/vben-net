using Microsoft.AspNetCore.Mvc.Controllers;

namespace Vben.Common.Sqlsugar.Config;

/// <summary>
/// SqlSugar 工作单元拦截器
/// </summary>
public class TransactionalFilter : IAsyncActionFilter, IOrderedFilter
{
    /// <summary>
    /// 过滤器排序
    /// </summary>
    internal const int FilterOrder = 9999;

    /// <summary>
    /// 排序属性
    /// </summary>
    public int Order => FilterOrder;

    /// <summary>
    /// SqlSugar 对象
    /// </summary>
    private readonly SqlSugarScope _sqlSugarClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sqlSugarClient"></param>
    public TransactionalFilter(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = (SqlSugarScope)sqlSugarClient;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Console.WriteLine("HashCode:"+_sqlSugarClient.GetHashCode());
        // 获取动作方法描述器
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var method = actionDescriptor.MethodInfo;

        // 判断是否贴有工作单元特性
        Console.WriteLine(typeof(TransactionalAttribute));
        if (!method.IsDefined(typeof(TransactionalAttribute), true))
        {
            // 调用方法
            _ = await next();
        }
        else
        {
            var attribute =
                (method.GetCustomAttributes(typeof(TransactionalAttribute), true).FirstOrDefault() as
                    TransactionalAttribute);

            // 开启事务
            await _sqlSugarClient.Ado.BeginTranAsync(attribute.IsolationLevel);

            // 调用方法
            var resultContext = await next();

            if (resultContext.Exception == null)
            {
                try
                {
                    await _sqlSugarClient.Ado.CommitTranAsync();
                }
                catch
                {
                    await _sqlSugarClient.Ado.RollbackTranAsync();
                }
                finally
                {
                    _sqlSugarClient.Ado.Dispose();
                }
            }
            else
            {
                // 回滚事务
                await _sqlSugarClient.Ado.RollbackTranAsync();
                _sqlSugarClient.Ado.Dispose();
            }
        }
    }
}