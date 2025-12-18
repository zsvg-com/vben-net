namespace Vben.Base.Mon.Server;

public static partial class Extensions
{

    /// <summary>
    /// 将object转换为long，若转换失败，则返回0。不抛出异常。  
    /// </summary>
    public static long ParseToLong(this object obj)
    {
        try
        {
            return long.Parse(obj.ToString());
        }
        catch
        {
            return 0L;
        }
    }

    #region 转换为double

    /// <summary>
    /// 将object转换为double，若转换失败，则返回0。不抛出异常。  
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object obj)
    {
        try
        {
            return double.Parse(obj.ToString());
        }
        catch
        {
            return 0;
        }
    }

    #endregion


    /// <summary>
    /// 将string转换为DateTime，若转换失败，则返回日期最小值。不抛出异常。  
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.MinValue;
            }

            if (str.Contains("-") || str.Contains("/"))
            {
                return DateTime.Parse(str);
            }
            else
            {
                int length = str.Length;
                switch (length)
                {
                    case 4:
                        return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    case 6:
                        return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                    case 8:
                        return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    case 10:
                        return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);
                    case 12:
                        return DateTime.ParseExact(str, "yyyyMMddHHmm",
                            System.Globalization.CultureInfo.CurrentCulture);
                    case 14:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss",
                            System.Globalization.CultureInfo.CurrentCulture);
                    default:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss",
                            System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
}