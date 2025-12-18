namespace Vben.Base.Tool.Num;

public class NumUtil
{
    // 利用给定的流水位生成第一个流水号
    public static string GetFirstSerialNum(int zlength)
    {
        string serialNum = "";
        //流水号为4里下面取3
        for (int i = 0; i < zlength - 1; i++)
        {
            serialNum = serialNum + "0";
        }
        serialNum = serialNum + "1";
        return serialNum;
    }

    //根据当前的流水号，生成下一个流水号
    public static string getNextSerialNum(string curSerialNum)
    {
        curSerialNum = "1" + curSerialNum;
        int icurSerialNum = int.Parse(curSerialNum);
        icurSerialNum++;
        curSerialNum = icurSerialNum + "";
        curSerialNum = curSerialNum.Substring(1);
        return curSerialNum;
    }

    public static string getNum(string numNow, string hintType)
    {
        string numNext = "";
        string todayYYYYMM = DateTime.Now.ToString("yyyyMM");
        if ("yyyymmxx" == hintType)
        {
            if (numNow == null)
            {
                numNext = todayYYYYMM + GetFirstSerialNum(2);
            }
            else
            {
                string yyyymm = numNow.Substring(0, 6);
                if (todayYYYYMM == yyyymm)
                {
                    numNext = yyyymm + getNextSerialNum(numNow.Substring(numNow.Length - 2));
                }
                else
                {
                    numNext = todayYYYYMM + GetFirstSerialNum(2);
                }
            }
        }
        else if ("xxxxxx" == hintType)
        {
            if (numNow == null)
            {
                numNext = GetFirstSerialNum(6);
            }
            else if (numNow.Length != 6)
            {
                numNext = GetFirstSerialNum(6);
            }
            else
            {
                numNext = getNextSerialNum(numNow);
            }
        }
        return numNext;
    }
}