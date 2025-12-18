using Microsoft.AspNetCore.Mvc.Filters;
using Vben.Common.Core.Filter;

namespace Vben.Common.Core.Wrapper;

public class ModelValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new ValidationError
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(err => err.ErrorMessage).ToList()
                })
                .ToList();
                
            var apiResult = ApiResult.Fail("数据验证失败", 400, errors);
            context.Result = new BadRequestObjectResult(apiResult);
            return;
        }
            
        await next();
    }
}
    
public class ValidationError
{
    public string Field { get; set; }
    public List<string> Errors { get; set; }
}