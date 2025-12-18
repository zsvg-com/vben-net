using System.Text.Json;
using Vben.Common.Core.Filter;

namespace Vben.Common.Core.Wrapper;

/// <summary>
    /// 全局响应包装中间件
    /// </summary>
    public class GlobalWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GlobalWrapperOptions _options;

        public GlobalWrapperMiddleware(
            RequestDelegate next,
            IOptions<GlobalWrapperOptions> options = null)
        {
            _next = next;
            _options = options?.Value ?? new GlobalWrapperOptions();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 检查是否应该跳过包装
            if (ShouldSkipWrapper(context))
            {
                await _next(context);
                return;
            }

            // 捕获原始响应流
            var originalBodyStream = context.Response.Body;
            
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);

                    // 处理不同类型的响应
                    await HandleResponse(context, memoryStream, originalBodyStream);
                }
                catch (Exception ex)
                {
                    // 异常处理
                    await HandleException(context, ex, originalBodyStream);
                }
                finally
                {
                    context.Response.Body = originalBodyStream;
                }
            }
        }

        private async Task HandleResponse(HttpContext context, MemoryStream memoryStream, Stream originalBodyStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            
            // 设置Content-Type
            context.Response.ContentType = "application/json";
            
            // 如果响应已经是包装格式，直接返回
            if (IsAlreadyWrapped(responseBody))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
                return;
            }
            
            // 包装响应
            var wrappedResult = WrapResponse(context, responseBody);
            var jsonResponse = JsonSerializer.Serialize(wrappedResult, 
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            // 重置响应
            context.Response.Body = originalBodyStream;
            await context.Response.WriteAsync(jsonResponse);
        }

        private object WrapResponse(HttpContext context, string responseBody)
        {
            int statusCode = context.Response.StatusCode;
            string message = GetMessageByStatusCode(statusCode);
            
            // 尝试解析响应体
            object data = responseBody;
            try
            {
                if (!string.IsNullOrEmpty(responseBody))
                {
                    data = JsonSerializer.Deserialize<JsonElement>(responseBody);
                }
            }
            catch
            {
                // 保持原样
            }
            
            // 根据HTTP状态码决定业务状态码
            int businessCode = statusCode == 200 ? 200 : statusCode;
            
            return new ApiResult
            {
                Code = businessCode,
                Msg = message,
                Data = data,
                Timestamp = DateTime.Now
            };
        }

        private async Task HandleException(HttpContext context, Exception ex, Stream originalBodyStream)
        {
            // _logger.LogError(ex, "全局包装器捕获异常");
            Console.WriteLine("全局包装器捕获异常");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
           
            var apiResult = new ApiResult
            {
                Code = 500,
                Msg = _options.ShowExceptionDetails ? ex.Message : "服务器内部错误",
                Data = _options.ShowExceptionDetails ? new { exception = ex.ToString() } : null,
                Timestamp = DateTime.Now
            };
            
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var jsonResponse = JsonSerializer.Serialize(apiResult,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            await context.Response.WriteAsync(jsonResponse);
        }

        private bool ShouldSkipWrapper(HttpContext context)
        {
            // 检查请求路径是否在排除列表中
            var path = context.Request.Path.Value?.ToLower();
            
            // 排除Swagger、健康检查、静态文件等
            if (path?.Contains("/swagger") == true ||
                path?.Contains("/health") == true ||
                path?.Contains("/hangfire") == true ||
                path?.EndsWith(".js") == true ||
                path?.EndsWith(".css") == true ||
                path?.EndsWith(".html") == true)
            {
                return true;
            }
            
            // 检查特性标记
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<NoWrapperAttribute>() != null)
            {
                return true;
            }
            
            return false;
        }

        private bool IsAlreadyWrapped(string responseBody)
        {
            try
            {
                if (string.IsNullOrEmpty(responseBody)) return false;
                
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                return root.TryGetProperty("code", out _) && 
                       root.TryGetProperty("msg", out _) && 
                       root.TryGetProperty("data", out _);
            }
            catch
            {
                return false;
            }
        }

        private string GetMessageByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "操作成功",
                201 => "创建成功",
                204 => "无内容",
                400 => "请求参数错误",
                401 => "未授权",
                403 => "禁止访问",
                404 => "资源不存在",
                409 => "数据冲突",
                422 => "数据验证失败",
                500 => "服务器内部错误",
                502 => "网关错误",
                503 => "服务不可用",
                _ => statusCode >= 400 ? "操作失败" : "操作成功"
            };
        }
    }

    /// <summary>
    /// 中间件配置选项
    /// </summary>
    public class GlobalWrapperOptions
    {
        /// <summary>
        /// 是否显示异常详情（生产环境建议关闭）
        /// </summary>
        public bool ShowExceptionDetails { get; set; } = false;
        
        /// <summary>
        /// 排除路径列表
        /// </summary>
        public List<string> ExcludePaths { get; set; } = new List<string>();
    }

    /// <summary>
    /// 标记不需要包装的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoWrapperAttribute : System.Attribute { }