namespace Vben.Base.Tool.Num;

[Service]
public class ToolNumService : BaseService<ToolNum>
{

    public string GetDateNum(ToolNum num, string dateType)
    {
        //获取 是否被修改过或新添加的 字段的值
        //如果  是否被修改过或新添加的=='Y'
        if (num.nflag)
        {
            //生成第一个流水号 0001
            string firstSerialNum = NumUtil.GetFirstSerialNum(num.nulen);
            //计算下一个流水号 0002
            string nextSerialNum = NumUtil.getNextSerialNum(firstSerialNum);
            //获取系统的当前时间 格式yyyyMMdd  20220906
            // string curDate = DateFormatUtils.format(new Date(), dateType);
            string curDate = DateTime.Now.ToString(dateType); ;

            //生成客户编码
            //编码前缀+"-"+利用日期位格式生成当前的日期[yyyyMMdd ]+"-"+0001  c-20220914-0001
            string back = (num.nupre ?? "") + curDate + firstSerialNum;
            //修改代码规则表
            //下一个流水号="0002"
            num.nunex = nextSerialNum;
            //当前日期  20250907
            num.cudat = curDate;
            //是否被修改过='N'
            num.nflag = false;
            repo.Update(num);
            return back;
        }
        else
        {
            //是否被修改过或新添加的=='N'
            //获取代码规则表中的当前日期字段的值
            string curDate = num.cudat;
            //获取系统的当前日期
            // String sysCurDate = DateFormatUtils.format(new Date(), dateType);
            string sysCurDate = DateTime.Now.ToString(dateType); ;
            //如果代码规则表中的当前日期字段的值==系统的当前日期
            if (curDate == sysCurDate)
            {
                //获取下一个流水号 ="0002"
                string nextseq = num.nunex;
                //计算新的流水号 0003
                string nextSerialNum = NumUtil.getNextSerialNum(nextseq);
                //生成客户编码
                //编码前缀+"-"+利用日期位格式生成当前的日期[yyyyMMdd ]+"-"+0001
                string back = (num.nupre == null ? "" : num.nupre) + sysCurDate + nextseq;
                //修改代码规则表
                //下一个流水号="0003"
                num.nunex = nextSerialNum;
                //当前日期  20140908
                //是否被修改过='N'
                repo.Update(num);
                return back;
            }
            else
            { //如果代码规则表中的当前日期字段的值!=系统的当前日期、

                //生成第一个流水号 0001
                String firstSerialNum = NumUtil.GetFirstSerialNum(num.nulen);
                //计算下一个流水号 0002
                String nextSerialNum = NumUtil.getNextSerialNum(firstSerialNum);
                //生成客户编码
                //编码前缀+"-"+利用日期位格式生成当前的日期[yyyyMMdd ]+"-"+0001
                String back = (num.nupre == null ? "" : num.nupre) + sysCurDate + firstSerialNum;
                //修改代码规则表
                //下一个流水号="0002"
                num.nunex = nextSerialNum;
                //当前日期  20110915
                num.cudat = sysCurDate;
                //是否被修改过='N'
                num.nflag = false;
                repo.Update(num);
                return back;
            }
        }
    }

    public String GetPureNum(ToolNum num)
    {
        if (num.nflag)
        {
            String firstSerialNum = NumUtil.GetFirstSerialNum(num.nulen);
            String nextSerialNum = NumUtil.getNextSerialNum(firstSerialNum);
            num.nunex = nextSerialNum;
            num.nflag = false;
            repo.Update(num);
            return (num.nupre == null ? "" : num.nupre) + firstSerialNum;
        }
        else
        {
            String nextseq = num.nunex;
            String nextSerialNum = NumUtil.getNextSerialNum(nextseq);
            num.nunex = nextSerialNum;
            repo.Update(num);
            return (num.nupre == null ? "" : num.nupre) + nextseq;
        }
    }

    public String getNum(String id)
    {
        // AssNumMain num = repo.GetSingle(id);
        ToolNum num = repo.GetSingle(t => t.id == id);
        String number = "";
        switch (num.numod)
        {
            case "uuid":
                number = YitIdHelper.NextId() + "";
                break;
            case "nodate":
                number = GetPureNum(num);
                break;
            case "yyyymmdd":
                number = GetDateNum(num, "yyyyMMdd");
                break;
            case "yymmdd":
                number = GetDateNum(num, "yyMMdd");
                break;
            case "yyyymm":
                number = GetDateNum(num, "yyyyMM");
                break;
            case "yymm":
                number = GetDateNum(num, "yyMM");
                break;
            case "yy":
                number = GetDateNum(num, "yy");
                break;
        }
        return number;
    }

    public ToolNumService(SqlSugarRepository<ToolNum> repo)
    {
        this.repo = repo;
    }
}