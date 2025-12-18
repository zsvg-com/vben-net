namespace Vben.Common.Core.Utils;

public class StrUtils
{
    public static string EMPTY = "";
    public static string UpperFirst(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
    
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public static bool IsUrl(string url)
    {
        bool isValid = Uri.IsWellFormedUriString(url, UriKind.Relative);
        return isValid;
    }
    
    /// <summary>
    /// 同时替换字符串中的多个不同文本[citation:1]
    /// </summary>
    /// <param name="text">原始字符串</param>
    /// <param name="targets">要查找的字符串数组</param>
    /// <param name="replacements">替换成的字符串数组</param>
    /// <returns>替换后的字符串</returns>
    public static string ReplaceEach(string text, string[] targets, string[] replacements)
    {
        if (string.IsNullOrEmpty(text)) return text;
        if (targets == null || replacements == null) 
            throw new ArgumentNullException("目标数组和替换数组不能为null");
        if (targets.Length != replacements.Length) 
            throw new ArgumentException("目标数组和替换数组长度必须相同");

        StringBuilder result = new StringBuilder(text);
        
        // 遍历每个要查找的目标字符串[citation:1]
        for (int i = 0; i < targets.Length; i++)
        {
            string target = targets[i];
            string replacement = replacements[i];
            
            if (string.IsNullOrEmpty(target)) continue;
            
            // 替换当前目标字符串[citation:1]
            result.Replace(target, replacement);
        }
        
        return result.ToString();
    }
}