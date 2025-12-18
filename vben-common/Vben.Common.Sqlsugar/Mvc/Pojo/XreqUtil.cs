using Vben.Common.Core;

namespace Vben.Common.Sqlsugar.Mvc.Pojo;

public class XreqUtil
{
    public static PagingParam GetPp()
    {
        var pp = new PagingParam();
        // var xx = MyApp.ServiceProvider;
        // var yy = MyApp.Configuration;
        var request = MyApp.HttpContext.Request;
        var strPageSize = request.Query["pageSize"].ToString();
        if (!string.IsNullOrEmpty(strPageSize))
        {
            pp.pageSize = int.Parse(strPageSize);
        }
        var strPage = request.Query["pageNum"].ToString();
        if (!string.IsNullOrEmpty(strPage))
        {
            pp.page = int.Parse(strPage);
        }
        return pp;
    }
}

public class PagingParam
{
    public RefAsync<int> total { get; set; } = 0;

    public int page { get; set; } = 1;

    public int pageSize { get; set; } = 10;


}