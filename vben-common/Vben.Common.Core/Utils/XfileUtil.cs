using NPOI.SS.Util;

namespace Vben.Common.Core.Utils;

public class XfileUtil
{
    public static string GetFileSize(long fileS)
    {
        string size = "";
        DecimalFormat df = new DecimalFormat("#.00");
        if (fileS < 1024)
        {
            size = df.Format(fileS) + "B";
        }
        else if (fileS < 1048576)
        {
            size = df.Format((double)fileS / 1024) + "KB";
        }
        else if (fileS < 1073741824)
        {
            size = df.Format((double)fileS / 1048576) + "MB";
        }
        else
        {
            size = df.Format((double)fileS / 1073741824) + "GB";
        }
        return size;
    }
}