using Vben.Base.Tool.Dict.Data;
using Vben.Base.Tool.Dict.Main;
using Vben.Base.Tool.Num;
using Vben.Common.Sqlsugar.Config;

namespace Vben.Admin.Launch.Init;

public class ToolInit : ITransient
{
    private readonly SqlSugarRepository<ToolNum> _numRepo;

    private readonly SqlSugarRepository<ToolDictMain> _dictMainRepo;

    private readonly SqlSugarRepository<ToolDictData> _dictDataRepo;

    public ToolInit(SqlSugarRepository<ToolNum> numRepo,
        SqlSugarRepository<ToolDictMain> dictMainRepo,
        SqlSugarRepository<ToolDictData> dictDataRepo)
    {
        _numRepo = numRepo;
        _dictMainRepo = dictMainRepo;
        _dictDataRepo = dictDataRepo;
    }

    public async Task InitData()
    {
        await InitNum();
        await InitDict();
    }

    //流水号初始化
    private async Task InitNum()
    {
        List<ToolNum> numList = new List<ToolNum>();
        ToolNum demoNum = new ToolNum();
        demoNum.id = "DEMO";
        demoNum.name = "DEMO流水号";
        demoNum.numod = "yy";
        demoNum.nulen = 4;
        demoNum.nflag = true;
        demoNum.nupre = "D";
        numList.Add(demoNum);

        ToolNum projNum = new ToolNum();
        projNum.id = "PROJ";
        projNum.name = "项目流水号";
        projNum.numod = "yyyymmdd";
        projNum.nulen = 3;
        projNum.nflag = true;
        projNum.nupre = "PR";
        numList.Add(projNum);

        ToolNum custNum = new ToolNum();
        custNum.id = "CUST";
        custNum.name = "客户流水号";
        custNum.numod = "yyyymmdd";
        custNum.nulen = 3;
        custNum.nflag = true;
        custNum.nupre = "CU";
        numList.Add(custNum);

        ToolNum distNum = new ToolNum();
        distNum.id = "DIST";
        distNum.name = "渠道商流水号";
        distNum.numod = "nodate";
        distNum.nulen = 7;
        distNum.nflag = true;
        distNum.nupre = "DMS";
        distNum.nunex = "1000001";
        numList.Add(distNum);

        ToolNum distUserNum = new ToolNum();
        distUserNum.id = "DIST_USER";
        distUserNum.name = "渠道商流水号";
        distUserNum.numod = "nodate";
        distUserNum.nulen = 6;
        distUserNum.nflag = true;
        distUserNum.nupre = "d";
        distUserNum.nunex = "100001";
        numList.Add(distUserNum);

        await _numRepo.InsertRangeAsync(numList);


    }

    //数据字典初始化
    private async Task InitDict()
    {
        List<ToolDictMain> dictList = new List<ToolDictMain>();

        ToolDictMain demoDict = new ToolDictMain();
        demoDict.id = "DEMO_GRADE";
        demoDict.name = "DEMO资质等级";
        demoDict.avtag = true;
        dictList.Add(demoDict);

        ToolDictMain distGradeDict = new ToolDictMain();
        distGradeDict.id = "DIST_GRADE";
        distGradeDict.name = "渠道商资质等级";
        distGradeDict.avtag = true;
        dictList.Add(distGradeDict);

        await _dictMainRepo.InsertRangeAsync(dictList);


        List<ToolDictData> dictDataList = new List<ToolDictData>();
        ToolDictData demo1 = new ToolDictData();
        demo1.id = YitIdHelper.NextId() + "";
        demo1.dalab = "A";
        demo1.daval = "A级资质";
        demo1.avtag = true;
        demo1.ornum = 1;
        demo1.dicid = "DEMO_GRADE";
        dictDataList.Add(demo1);

        ToolDictData demo2 = new ToolDictData();
        demo2.id = YitIdHelper.NextId() + "";
        demo2.dalab = "B";
        demo2.daval = "B级资质";
        demo2.avtag = true;
        demo2.ornum = 2;
        demo2.dicid = "DEMO_GRADE";
        dictDataList.Add(demo2);

        ToolDictData demo3 = new ToolDictData();
        demo3.id = YitIdHelper.NextId() + "";
        demo3.dalab = "C";
        demo3.daval = "C级资质";
        demo3.avtag = true;
        demo3.ornum = 3;
        demo3.dicid = "DEMO_GRADE";
        dictDataList.Add(demo3);

        ToolDictData demo4 = new ToolDictData();
        demo4.id = YitIdHelper.NextId() + "";
        demo4.dalab = "Z";
        demo4.daval = "不合格";
        demo4.avtag = true;
        demo4.ornum = 4;
        demo4.dicid = "DEMO_GRADE";
        dictDataList.Add(demo4);

        ToolDictData dist1 = new ToolDictData();
        dist1.id = YitIdHelper.NextId() + "";
        dist1.dalab = "A";
        dist1.daval = "A级资质";
        dist1.avtag = true;
        dist1.ornum = 1;
        dist1.dicid = "DIST_GRADE";
        dictDataList.Add(dist1);

        ToolDictData dist2 = new ToolDictData();
        dist2.id = YitIdHelper.NextId() + "";
        dist2.dalab = "B";
        dist2.daval = "B级资质";
        dist2.avtag = true;
        dist2.ornum = 2;
        dist2.dicid = "DIST_GRADE";
        dictDataList.Add(dist2);

        ToolDictData dist3 = new ToolDictData();
        dist3.id = YitIdHelper.NextId() + "";
        dist3.dalab = "C";
        dist3.daval = "C级资质";
        dist3.avtag = true;
        dist3.ornum = 3;
        dist3.dicid = "DIST_GRADE";
        dictDataList.Add(dist3);

        ToolDictData dist4 = new ToolDictData();
        dist4.id = YitIdHelper.NextId() + "";
        dist4.dalab = "Z";
        dist4.daval = "不合格";
        dist4.avtag = true;
        dist4.ornum = 4;
        dist4.dicid = "DIST_GRADE";
        dictDataList.Add(dist4);

        await _dictDataRepo.InsertRangeAsync(dictDataList);
    }
}