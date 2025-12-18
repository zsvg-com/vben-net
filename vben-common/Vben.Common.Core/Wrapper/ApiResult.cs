namespace Vben.Common.Core.Wrapper;

 /// <summary>
    /// 统一API响应格式
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码（业务层面）
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 是否成功（Code=200表示成功）
        /// </summary>
        public bool Success => Code == 200;
        
        public ApiResult() { }
        
        public ApiResult(int code, string msg, T data)
        {
            Code = code;
            Msg = msg;
            Data = data;
        }
        
        /// <summary>
        /// 创建成功响应
        /// </summary>
        public static ApiResult<T> Ok(T data, string msg = "操作成功")
            => new ApiResult<T>(200, msg, data);
            
        /// <summary>
        /// 创建失败响应
        /// </summary>
        public static ApiResult<T> Fail(string msg, int code = 500, T data = default)
            => new ApiResult<T>(code, msg, data);
    }
    
    /// <summary>
    /// 非泛型版本
    /// </summary>
    public class ApiResult : ApiResult<object>
    {
        public ApiResult() { }
        
        public ApiResult(int code, string msg, object data = null) 
            : base(code, msg, data) { }
            
        public static ApiResult Ok(object data = null, string msg = "操作成功")
            => new ApiResult(200, msg, data);
            
        public static ApiResult Fail(string msg, int code = 500, object data = null)
            => new ApiResult(code, msg, data);
    }
    
    /// <summary>
    /// 分页数据包装
    /// </summary>
    public class PagedResult<T>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public int Pages => PageSize > 0 ? (int)Math.Ceiling(Total / (double)PageSize) : 0;
        public List<T> List { get; set; }
    }