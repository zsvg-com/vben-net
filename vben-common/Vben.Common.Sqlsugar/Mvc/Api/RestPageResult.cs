namespace Vben.Common.Sqlsugar.Mvc.Api;

public class PageResult
{
    public int total { get; set; }
    public object rows { get; set; }
}

public static class RestPageResult
{
    public static PageResult Build(int Total, object Rows)
    {
        return new()
        {
            total = Total,
            rows = Rows
        };
    }
}