// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Newtonsoft.Json;

namespace Admin.NET.Core;

/// <summary>
/// 对象拓展
/// </summary>
[SuppressSniffer]
public static partial class ObjectExtension
{
    /// <summary>
    /// 类型属性列表映射表
    /// </summary>
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    /// <summary>
    /// 脱敏特性缓存映射表
    /// </summary>
    private static readonly ConcurrentDictionary<PropertyInfo, DataMaskAttribute> AttributeCache = new();

    /// <summary>
    /// 判断类型是否实现某个泛型
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="generic">泛型类型</param>
    /// <returns>bool</returns>
    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        // 检查接口类型
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;

        // 检查类型
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }

        return false;

        // 判断逻辑
        bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }

    /// <summary>
    /// 将字典转化为QueryString格式
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="urlEncode"></param>
    /// <returns></returns>
    public static string ToQueryString(this Dictionary<string, string> dict, bool urlEncode = true)
    {
        return string.Join("&", dict.Select(p => $"{(urlEncode ? p.Key?.UrlEncode() : "")}={(urlEncode ? p.Value?.UrlEncode() : "")}"));
    }

    /// <summary>
    /// 将字符串URL编码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UrlEncode(this string str)
    {
        return string.IsNullOrEmpty(str) ? "" : System.Uri.EscapeDataString(str);
    }

    /// <summary>
    /// 对象序列化成Json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
        var jsonSettings = SetNewtonsoftJsonSetting();
        return JSON.GetJsonSerializer().Serialize(obj, jsonSettings);
    }

    private static JsonSerializerSettings SetNewtonsoftJsonSetting()
    {
        JsonSerializerSettings setting = new JsonSerializerSettings();
        setting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                                                                      //setting.ContractResolver = new HelErpContractResolver("StartTime", customName);
        return setting;
    }

    /// <summary>
    /// Json字符串反序列化成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T ToObject<T>(this string json)
    {
        return JSON.GetJsonSerializer().Deserialize<T>(json);
    }

    /// <summary>
    /// 将object转换为long，若失败则返回0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 将object转换为long，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long ParseToLong(this string str, long defaultValue)
    {
        try
        {
            return long.Parse(str);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将object转换为double，若失败则返回0
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

    /// <summary>
    /// 将object转换为double，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object str, double defaultValue)
    {
        try
        {
            return double.Parse(str.ToString());
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回日期最小值
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
            if (str.Contains('-') || str.Contains('/'))
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
                        return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                    case 14:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    default:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回默认值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str, DateTime? defaultValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue.GetValueOrDefault();
            }
            if (str.Contains('-') || str.Contains('/'))
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
                        return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                    case 14:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    default:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }
        catch
        {
            return defaultValue.GetValueOrDefault();
        }
    }

    /// <summary>
    /// 将 string 时间日期格式转换成字符串 如 {yyyy} => 2024
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ParseToDateTimeForRep(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            str = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";

        var date = DateTime.Now;
        var reg = new Regex(@"(\{.+?})");
        var match = reg.Matches(str);
        match.ToList().ForEach(u =>
        {
            var temp = date.ToString(u.ToString().Substring(1, u.Length - 2));
            str = str.Replace(u.ToString(), temp);
        });
        return str;
    }

    /// <summary>
    /// 是否有值
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object obj)
    {
        return obj == null || string.IsNullOrEmpty(obj.ToString());
    }

    /// <summary>
    /// 字符串掩码
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="mask">掩码符</param>
    /// <returns></returns>
    public static string Mask(this string str, char mask = '*')
    {
        if (string.IsNullOrWhiteSpace(str?.Trim()))
            return str;

        str = str.Trim();
        var masks = mask.ToString().PadLeft(4, mask);
        return str.Length switch
        {
            >= 11 => Regex.Replace(str, "(.{3}).*(.{4})", $"$1{masks}$2"),
            10 => Regex.Replace(str, "(.{3}).*(.{3})", $"$1{masks}$2"),
            9 => Regex.Replace(str, "(.{2}).*(.{3})", $"$1{masks}$2"),
            8 => Regex.Replace(str, "(.{2}).*(.{2})", $"$1{masks}$2"),
            7 => Regex.Replace(str, "(.{1}).*(.{2})", $"$1{masks}$2"),
            6 => Regex.Replace(str, "(.{1}).*(.{1})", $"$1{masks}$2"),
            _ => Regex.Replace(str, "(.{1}).*", $"$1{masks}")
        };
    }

    /// <summary>
    /// 身份证号掩码
    /// </summary>
    /// <param name="idCard">身份证号</param>
    /// <param name="mask">掩码符</param>
    /// <returns></returns>
    public static string MaskIdCard(this string idCard, char mask = '*')
    {
        if (!idCard.TryValidate(ValidationTypes.IDCard).IsValid) return idCard;

        var masks = mask.ToString().PadLeft(8, mask);
        return Regex.Replace(idCard, @"^(.{6})(.*)(.{4})$", $"$1{masks}$3");
    }

    /// <summary>
    /// 邮箱掩码
    /// </summary>
    /// <param name="email">邮箱</param>
    /// <param name="mask">掩码符</param>
    /// <returns></returns>
    public static string MaskEmail(this string email, char mask = '*')
    {
        if (!email.TryValidate(ValidationTypes.EmailAddress).IsValid) return email;

        var pos = email.IndexOf("@");
        return Mask(email[..pos], mask) + email[pos..];
    }

    /// <summary>
    /// 将字符串转为值类型，若没有得到或者错误返回为空
    /// </summary>
    /// <typeparam name="T">指定值类型</typeparam>
    /// <param name="str">传入字符串</param>
    /// <returns>可空值</returns>
    public static T? ParseTo<T>(this string str) where T : struct
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                MethodInfo method = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                if (method != null)
                {
                    T result = (T)method.Invoke(null, new string[] { str });
                    return result;
                }
            }
        }
        catch
        {
        }
        return null;
    }

    /// <summary>
    /// 将字符串转为值类型，若没有得到或者错误返回为空
    /// </summary>
    /// <param name="str">传入字符串</param>
    /// <param name="type">目标类型</param>
    /// <returns>可空值</returns>
    public static object ParseTo(this string str, Type type)
    {
        try
        {
            if (type.Name == "String")
                return str;

            if (!string.IsNullOrWhiteSpace(str))
            {
                var _type = type;
                if (type.Name.StartsWith("Nullable"))
                    _type = type.GetGenericArguments()[0];

                MethodInfo method = _type.GetMethod("Parse", new Type[] { typeof(string) });
                if (method != null)
                    return method.Invoke(null, new string[] { str });
            }
        }
        catch
        {
        }
        return null;
    }

    /// <summary>
    /// 将一个对象属性值赋给另一个指定对象属性, 只复制相同属性的
    /// </summary>
    /// <param name="src">原数据对象</param>
    /// <param name="target">目标数据对象</param>
    /// <param name="changeProperties">属性集，键为原属性，值为目标属性</param>
    /// <param name="unChangeProperties">属性集，目标不修改的属性</param>
    public static void CopyTo(object src, object target, Dictionary<string, string>? changeProperties = null, string[]? unChangeProperties = null)
    {
        if (src == null || target == null)
            throw new ArgumentException("src == null || target == null ");

        var SourceType = src.GetType();
        var TargetType = target.GetType();

        if (changeProperties == null || changeProperties.Count == 0)
        {
            var fields = TargetType.GetProperties();
            changeProperties = fields.Select(m => m.Name).ToDictionary(m => m);
        }

        if (unChangeProperties == null || unChangeProperties.Length == 0)
        {
            foreach (var item in changeProperties)
            {
                var srcProperty = SourceType.GetProperty(item.Key);
                if (srcProperty != null)
                {
                    var sourceVal = srcProperty.GetValue(src, null);

                    var tarProperty = TargetType.GetProperty(item.Value);
                    tarProperty?.SetValue(target, sourceVal, null);
                }
            }
        }
        else
        {
            foreach (var item in changeProperties)
            {
                if (!unChangeProperties.Any(m => m == item.Value))
                {
                    var srcProperty = SourceType.GetProperty(item.Key);
                    if (srcProperty != null)
                    {
                        var sourceVal = srcProperty.GetValue(src, null);

                        var tarProperty = TargetType.GetProperty(item.Value);
                        tarProperty?.SetValue(target, sourceVal, null);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 深复制
    /// </summary>
    /// <typeparam name="T">深复制源对象</typeparam>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static T DeepCopy<T>(this T obj)
    {
        var jsonSettings = SetNewtonsoftJsonSetting();
        var json = JSON.Serialize(obj, jsonSettings);
        return JSON.Deserialize<T>(json);
    }

    /// <summary>
    /// 对带有<see cref="DataMaskAttribute"/>特性字段进行脱敏处理
    /// </summary>
    public static T MaskSensitiveData<T>(this T obj) where T : class
    {
        if (obj == null) return null;

        var type = typeof(T);

        // 获取或缓存属性集合
        var properties = PropertyCache.GetOrAdd(type, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.GetCustomAttribute<DataMaskAttribute>() != null)
                .ToArray());

        // 并行处理可写属性
        Parallel.ForEach(properties, prop =>
        {
            if (!prop.CanWrite) return;

            // 获取或缓存特性
            var maskAttr = AttributeCache.GetOrAdd(prop, p => p.GetCustomAttribute<DataMaskAttribute>());

            if (maskAttr == null) return;

            // 处理非空字符串
            if (prop.GetValue(obj) is string { Length: > 0 } value)
            {
                prop.SetValue(obj, maskAttr.Mask(value));
            }
        });

        return obj;
    }
}