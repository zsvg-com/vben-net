using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Admin.Launch.Init;

public class DataInit(
    SysOrgInit orgInit,
    SysPermInit permInit,
    ToolInit toolInit,
    SqlSugarRepository<SysOrg> sysOrgRepo)
    : ITransient
{
    //首次启动，数据库生成后，初始化组织架构，菜单，权限角色,接口等信息
    public async Task Init()
    {
        var sysOrg = sysOrgRepo.GetSingle(it => it.id == "u1");
        if (sysOrg == null)
        {
            Console.WriteLine("首次启动系统，正在进行数据库初始化，请耐心等待。");
            await orgInit.initData();
            Console.WriteLine("1 初始化组长架构完毕");
            await permInit.initData();
            Console.WriteLine("2 初始化权限完毕");
            await toolInit.InitData();
            Console.WriteLine("3 初始化辅助工具完毕");
        }
    }

}