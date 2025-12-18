namespace Vben.Common.Core.Utils;

public class XstrUtil
{
    /// <summary>将大驼峰命名转为蛇形命名</summary>
    public static string RenameSnakeCase(string str)
    {
        var builder = new StringBuilder();
        var name = str;
        var previousUpper = false;
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && !previousUpper)
                {
                    builder.Append("_");
                }
                builder.Append(char.ToLowerInvariant(c));
                previousUpper = true;
            }
            else
            {
                builder.Append(c);
                previousUpper = false;
            }
        }
        return builder.ToString();
    }

    /// <summary>将大驼峰命名转为蛇形命名</summary>
    public static string RenameUrlCase(string str)
    {
        var builder = new StringBuilder();
        var name = str;
        var previousUpper = false;
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && !previousUpper)
                {
                    builder.Append("/");
                }
                builder.Append(char.ToLowerInvariant(c));
                previousUpper = true;
            }
            else
            {
                builder.Append(c);
                previousUpper = false;
            }
        }
        return builder.ToString();
    }

    public static string FirstCharToUpper(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input.First().ToString().ToUpper() + input.Substring(1);
    }

    public static string LineToHump(string name)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var s in name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries))
        {
            builder.Append(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s));
        }

        return builder.ToString();
    }
}