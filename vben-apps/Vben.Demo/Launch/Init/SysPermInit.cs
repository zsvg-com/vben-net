using Vben.Base.Sys.Perm.Api;
using Vben.Base.Sys.Perm.Menu;
using Vben.Base.Sys.Perm.Role;

namespace Vben.Admin.Launch.Init;

public class SysPermInit : ITransient
{
    private readonly SqlSugarRepository<SysPermMenu> _menuRepo;
    private readonly SqlSugarRepository<SysPermApi> _apiRepo;
    private readonly SqlSugarRepository<SysPermRole> _roleRepo;

    public SysPermInit(
        SqlSugarRepository<SysPermMenu> menuRepo,
        SqlSugarRepository<SysPermApi> apiRepo,
        SqlSugarRepository<SysPermRole> roleRepo
        )
    {
        _menuRepo = menuRepo;
        _apiRepo = apiRepo;
        _roleRepo = roleRepo;
    }

    public async Task initData()
    {
        await initBaseMenu();
        await initBpmMenu();
        await initBaseApi();
        await initBpmApi();
        await initDemo();
        await initRole();
    }


    private async Task initBaseMenu()
    {
        List<SysPermMenu> list = new List<SysPermMenu>();
        
        SysPermMenu m1000 = new SysPermMenu();
        m1000.id=1000L;
        m1000.icon="tdesign:system-setting";
        m1000.name="系统管理";
        m1000.ornum=1000;
        m1000.path="sys";
        m1000.pid=0L;
        m1000.type="1";
        m1000.comp="Layout";
        m1000.catag=false;
        m1000.shtag=true;
        m1000.outag=false;
        list.Add(m1000);
        
        SysPermMenu m1010 = new SysPermMenu();
        m1010.id=1010L;
        m1010.comp="sys/org/dept/index";
        m1010.icon="mingcute:department-line";
        m1010.name="部门管理";
        m1010.ornum=1010;
        m1010.path="org/dept";
        m1010.pid=1000L;
        m1010.catag=false;
        m1010.shtag=true;
        m1010.outag=false;
        list.Add(m1010);
        
        SysPermMenu m1020 = new SysPermMenu();
        m1020.id=1020L;
        m1020.comp="sys/org/user/index";
        m1020.icon="ant-design:user-outlined";
        m1020.name="用户管理";
        m1020.ornum=1020;
        m1020.path="org/user";
        m1020.pid=1000L;
        m1020.catag=false;
        m1020.shtag=true;
        m1020.outag=false;
        list.Add(m1020);
        
        SysPermMenu m1021 = new SysPermMenu();
        m1021.id=1021L;
        m1021.comp="sys/org/user/tedit";
        m1021.icon="mingcute:user-edit-line";
        m1021.name="用户编辑";
        m1021.ornum=1021;
        m1021.path="org/user/edit";
        m1021.pid=1000L;
        m1021.catag=true;
        m1021.shtag=false;
        m1021.outag=false;
        list.Add(m1021);
        
        SysPermMenu m1030 = new SysPermMenu();
        m1030.id=1030L;
        m1030.comp="sys/org/post/index";
        m1030.icon="icon-park-outline:appointment";
        m1030.name="岗位管理";
        m1030.ornum=1030;
        m1030.path="org/post";
        m1030.pid=1000L;
        m1030.catag=false;
        m1030.shtag=true;
        m1030.outag=false;
        list.Add(m1030);
        
        SysPermMenu m1040 = new SysPermMenu();
        m1040.id=1040L;
        m1040.comp="sys/org/group/index";
        m1040.icon="material-symbols:group-outline-rounded";
        m1040.name="群组管理";
        m1040.ornum=1040;
        m1040.path="org/group";
        m1040.pid=1000L;
        m1040.catag=false;
        m1040.shtag=true;
        m1040.outag=false;
        list.Add(m1040);
        
        SysPermMenu m1050 = new SysPermMenu();
        m1050.id=1050L;
        m1050.comp="sys/perm/menu/index";
        m1050.icon="ri:menu-fold-2-fill";
        m1050.name="菜单管理";
        m1050.ornum=1050;
        m1050.path="perm/menu";
        m1050.pid=1000L;
        m1050.catag=false;
        m1050.shtag=true;
        m1050.outag=false;
        list.Add(m1050);
        
        SysPermMenu m1060 = new SysPermMenu();
        m1060.id=1060L;
        m1060.comp="sys/perm/api/index";
        m1060.icon="ant-design:api-outlined";
        m1060.name="接口管理";
        m1060.ornum=1060;
        m1060.path="perm/api";
        m1060.pid=1000L;
        m1060.catag=false;
        m1060.shtag=true;
        m1060.outag=false;
        list.Add(m1060);
        
        SysPermMenu m1070 = new SysPermMenu();
        m1070.id=1070L;
        m1070.comp="sys/perm/role/index";
        m1070.icon="eos-icons:role-binding-outlined";
        m1070.name="角色管理";
        m1070.ornum=1070;
        m1070.path="perm/role";
        m1070.pid=1000L;
        m1070.catag=false;
        m1070.shtag=true;
        m1070.outag=false;
        list.Add(m1070);
        
        SysPermMenu m1071 = new SysPermMenu();
        m1071.id=1071L;
        m1071.comp="sys/perm/role/edit";
        m1071.icon="oui:app-users-roles";
        m1071.name="角色编辑";
        m1071.ornum=1071;
        m1071.path="perm/role/edit";
        m1071.pid=1000L;
        m1071.catag=true;
        m1071.shtag=false;
        m1071.outag=false;
        list.Add(m1071);
        
        SysPermMenu m1080 = new SysPermMenu();
        m1080.id=1080L;
        m1080.comp="sys/config/index";
        m1080.icon="ant-design:setting-outlined";
        m1080.name="参数设置";
        m1080.ornum=1080;
        m1080.path="config";
        m1080.pid=1000L;
        m1080.catag=false;
        m1080.shtag=true;
        m1080.outag=false;
        list.Add(m1080);
        
        SysPermMenu m1090 = new SysPermMenu();
        m1090.id=1090L;
        m1090.comp="sys/notice/index";
        m1090.icon="fe:notice-push";
        m1090.name="通知公告";
        m1090.ornum=1090;
        m1090.path="notice";
        m1090.pid=1000L;
        m1090.catag=false;
        m1090.shtag=true;
        m1090.outag=false;
        list.Add(m1090);
        
        SysPermMenu m2000 = new SysPermMenu();
        m2000.id=2000L;
        m2000.comp="Layout";
        m2000.icon="eos-icons:monitoring";
        m2000.name="监控中心";
        m2000.ornum=2000;
        m2000.path="mon";
        m2000.pid=0L;
        m2000.type="1";
        m2000.catag=false;
        m2000.shtag=true;
        m2000.outag=false;
        list.Add(m2000);
        
        SysPermMenu m2010 = new SysPermMenu();
        m2010.id=2010L;
        m2010.comp="mon/online/user/index";
        m2010.icon="oui:online";
        m2010.name="在线用户";
        m2010.ornum=2010;
        m2010.path="online/user";
        m2010.pid=2000L;
        m2010.catag=false;
        m2010.shtag=true;
        m2010.outag=false;
        list.Add(m2010);
        
        SysPermMenu m2020 = new SysPermMenu();
        m2020.id=2020L;
        m2020.comp="mon/login/log/index";
        m2020.icon="uiw:login";
        m2020.name="登录日志";
        m2020.ornum=2020;
        m2020.path="login/log";
        m2020.pid=2000L;
        m2020.catag=false;
        m2020.shtag=true;
        m2020.outag=false;
        list.Add(m2020);
        
        SysPermMenu m2030 = new SysPermMenu();
        m2030.id=2030L;
        m2030.comp="mon/oper/log/index";
        m2030.icon="icon-park-outline:reverse-operation-in";
        m2030.name="操作日志";
        m2030.ornum=2030;
        m2030.path="oper/log";
        m2030.pid=2000L;
        m2030.catag=false;
        m2030.shtag=true;
        m2030.outag=false;
        list.Add(m2030);
        
        SysPermMenu m2040 = new SysPermMenu();
        m2040.id=2040L;
        m2040.comp="mon/server/net";
        m2040.icon="mdi:server-outline";
        m2040.name="服务监控";
        m2040.ornum=2040;
        m2040.path="server";
        m2040.pid=2000L;
        m2040.catag=false;
        m2040.shtag=true;
        m2040.outag=false;
        list.Add(m2040);
        
        SysPermMenu m2050 = new SysPermMenu();
        m2050.id=2050L;
        m2050.comp="mon/cache/index";
        m2050.icon="octicon:cache-24";
        m2050.name="缓存监控";
        m2050.ornum=2050;
        m2050.path="cache";
        m2050.pid=2000L;
        m2050.catag=false;
        m2050.shtag=true;
        m2050.outag=false;
        list.Add(m2050);
        
        SysPermMenu m2060 = new SysPermMenu();
        m2060.id=2060L;
        m2060.comp="mon/job/main/index";
        m2060.icon="streamline:task-list";
        m2060.name="定时任务";
        m2060.ornum=2060;
        m2060.path="job/main";
        m2060.pid=2000L;
        m2060.catag=false;
        m2060.shtag=true;
        m2060.outag=false;
        list.Add(m2060);
        
        SysPermMenu m2061 = new SysPermMenu();
        m2061.id=2061L;
        m2061.comp="mon/job/log/index";
        m2061.icon="ix:log";
        m2061.name="任务日志";
        m2061.ornum=2061;
        m2061.path="job/log";
        m2061.pid=2000L;
        m2061.catag=false;
        m2061.shtag=false;
        m2061.outag=false;
        list.Add(m2061);
        
        SysPermMenu m3000 = new SysPermMenu();
        m3000.id=3000L;
        m3000.comp="Layout";
        m3000.icon="ant-design:tool-outlined";
        m3000.name="辅助工具";
        m3000.ornum=3000;
        m3000.path="tool";
        m3000.pid=0L;
        m3000.type="1";
        m3000.catag=false;
        m3000.shtag=true;
        m3000.outag=false;
        list.Add(m3000);
        
        SysPermMenu m3010 = new SysPermMenu();
        m3010.id=3010L;
        m3010.comp="tool/dict/index";
        m3010.icon="fluent-mdl2:dictionary";
        m3010.name="字典工具";
        m3010.ornum=3010;
        m3010.path="dict";
        m3010.pid=3000L;
        m3010.catag=false;
        m3010.shtag=true;
        m3010.outag=false;
        list.Add(m3010);
        
        SysPermMenu m3020 = new SysPermMenu();
        m3020.id=3020L;
        m3020.comp="tool/num/index";
        m3020.icon="streamline-sharp:steps-number";
        m3020.name="编号工具";
        m3020.ornum=3020;
        m3020.path="num";
        m3020.pid=3000L;
        m3020.catag=false;
        m3020.shtag=true;
        m3020.outag=false;
        list.Add(m3020);
        
        SysPermMenu m3030 = new SysPermMenu();
        m3030.id=3030L;
        m3030.comp="tool/oss/main/index";
        m3030.icon="mdi:file-outline";
        m3030.name="文件工具";
        m3030.ornum=3030;
        m3030.path="oss";
        m3030.pid=3000L;
        m3030.catag=false;
        m3030.shtag=true;
        m3030.outag=false;
        list.Add(m3030);
        
        SysPermMenu m3040 = new SysPermMenu();
        m3040.id=3040L;
        m3040.comp="tool/form/index";
        m3040.icon="fluent:form-20-regular";
        m3040.name="在线表单";
        m3040.ornum=3040;
        m3040.path="form";
        m3040.pid=3000L;
        m3040.catag=false;
        m3040.shtag=true;
        m3040.outag=false;
        list.Add(m3040);
        
        SysPermMenu m3041 = new SysPermMenu();
        m3041.id=3041L;
        m3041.comp="tool/form/edit";
        m3041.icon="fluent:form-20-regular";
        m3041.name="在线表单";
        m3041.ornum=3041;
        m3041.path="form/edit";
        m3041.pid=3000L;
        m3041.catag=true;
        m3041.shtag=false;
        m3041.outag=false;
        list.Add(m3041);
        
        SysPermMenu m3050 = new SysPermMenu();
        m3050.id=3050L;
        m3050.comp="tool/code/index";
        m3050.icon="humbleicons:code";
        m3050.name="代码生成";
        m3050.ornum=3050;
        m3050.path="code";
        m3050.pid=3000L;
        m3050.catag=false;
        m3050.shtag=true;
        m3050.outag=false;
        list.Add(m3050);
        
        SysPermMenu m3051 = new SysPermMenu();
        m3051.id=3051L;
        m3051.comp="tool/code/edit";
        m3051.icon="humbleicons:code";
        m3051.name="代码生成";
        m3051.ornum=3051;
        m3051.path="code/edit";
        m3051.pid=3000L;
        m3051.catag=true;
        m3051.shtag=false;
        m3051.outag=false;
        list.Add(m3051);
        
        foreach (SysPermMenu item in list)
        {
            item.avtag=true;
            if(item.type==null){
                item.type="2";
            }
            item.uptim=item.crtim;
        }
        await _menuRepo.InsertRangeAsync(list);
    }

    private async Task initBpmMenu()
    {
        List<SysPermMenu> list = new List<SysPermMenu>();

        SysPermMenu m6000 = new SysPermMenu();
        m6000.id = 6000L;
        m6000.icon = "streamline-sharp:text-flow-rows";
        m6000.name = "流程管理";
        m6000.ornum = 6000;
        m6000.path = "bpm";
        m6000.pid = 0L;
        m6000.type = "1";
        m6000.comp = "Layout";
        m6000.catag = false;
        m6000.shtag = true;
        m6000.outag = false;
        list.Add(m6000);

        SysPermMenu m6010 = new SysPermMenu();
        m6010.id = 6010L;
        m6010.comp = "bpm/bus/cate/index";
        m6010.icon = "tabler:category-plus";
        m6010.name = "流程分类";
        m6010.ornum = 6010;
        m6010.path = "bus/cate";
        m6010.pid = 6000L;
        m6010.catag = false;
        m6010.shtag = true;
        m6010.outag = false;
        list.Add(m6010);
        
        SysPermMenu m6020 = new SysPermMenu();
        m6020.id = 6020L;
        m6020.comp = "bpm/bus/tmpl/index";
        m6020.icon = "carbon:prompt-template";
        m6020.name = "流程模板";
        m6020.ornum = 6020;
        m6020.path = "bus/tmpl";
        m6020.pid = 6000L;
        m6020.catag = false;
        m6020.shtag = true;
        m6020.outag = false;
        list.Add(m6020);
        
        SysPermMenu m6021 = new SysPermMenu();
        m6021.id = 6021L;
        m6021.comp = "bpm/bus/tmpl/edit";
        m6021.icon = "carbon:prompt-template";
        m6021.name = "流程模板编辑";
        m6021.ornum = 6021;
        m6021.path = "bus/tmpl/edit";
        m6021.pid = 6000L;
        m6021.catag = true;
        m6021.shtag = false;
        m6021.outag = false;
        list.Add(m6021);
        
        SysPermMenu m6030 = new SysPermMenu();
        m6030.id = 6030L;
        m6030.comp = "bpm/bus/main/index";
        m6030.icon = "ri:instance-line";
        m6030.name = "流程清单";
        m6030.ornum = 6030;
        m6030.path = "bus/main";
        m6030.pid = 6000L;
        m6030.catag = false;
        m6030.shtag = true;
        m6030.outag = false;
        list.Add(m6030);
        
        SysPermMenu m6031 = new SysPermMenu();
        m6031.id = 6031L;
        m6031.comp = "bpm/bus/main/edit";
        m6031.icon = "ri:instance-line";
        m6031.name = "流程编辑";
        m6031.ornum = 6031;
        m6031.path = "bus/main/edit";
        m6031.pid = 6000L;
        m6031.catag = true;
        m6031.shtag = false;
        m6031.outag = false;
        list.Add(m6031);
        
        SysPermMenu m6032 = new SysPermMenu();
        m6032.id = 6032L;
        m6032.comp = "bpm/bus/main/view";
        m6032.icon = "ri:instance-line";
        m6032.name = "流程查看";
        m6032.ornum = 6032;
        m6032.path = "bus/main/view";
        m6032.pid = 6000L;
        m6032.catag = true;
        m6032.shtag = false;
        m6032.outag = false;
        list.Add(m6032);
        
        SysPermMenu m6040 = new SysPermMenu();
        m6040.id = 6040L;
        m6040.comp = "bpm/todo/index";
        m6040.icon = "ri:todo-line";
        m6040.name = "流程待办";
        m6040.ornum = 6040;
        m6040.path = "todo";
        m6040.pid = 6000L;
        m6040.catag = false;
        m6040.shtag = true;
        m6040.outag = false;
        list.Add(m6040);
        
        SysPermMenu m6050 = new SysPermMenu();
        m6050.id = 6050L;
        m6050.comp = "bpm/org/tree/index";
        m6050.icon = "mdi:workflow-outline";
        m6050.name = "流程组织";
        m6050.ornum = 6050;
        m6050.path = "org/tree";
        m6050.pid = 6000L;
        m6050.catag = false;
        m6050.shtag = true;
        m6050.outag = false;
        list.Add(m6050);
        
        SysPermMenu m6051 = new SysPermMenu();
        m6051.id = 6051L;
        m6051.comp = "bpm/org/node/index";
        m6051.icon = "mdi:workflow-outline";
        m6051.name = "流程组织节点";
        m6051.ornum = 6051;
        m6051.path = "org/node";
        m6051.pid = 6000L;
        m6051.catag = false;
        m6051.shtag = false;
        m6051.outag = false;
        list.Add(m6051);
        
        foreach (SysPermMenu item in list)
        {
            item.avtag=true;
            if(item.type==null){
                item.type="2";
            }
            item.uptim=item.crtim;
        }
        await _menuRepo.InsertRangeAsync(list);
    }
    
     private async Task initDemo()
    {
        List<SysPermMenu> list = new List<SysPermMenu>();

        SysPermMenu m8000 = new SysPermMenu();
        m8000.id = 8000L;
        m8000.icon = "hugeicons:star";
        m8000.name = "使用案例";
        m8000.ornum = 8000;
        m8000.path = "demo";
        m8000.pid = 0L;
        m8000.type = "1";
        m8000.comp = "Layout";
        m8000.catag = false;
        m8000.shtag = true;
        m8000.outag = false;
        list.Add(m8000);

        SysPermMenu m8010 = new SysPermMenu();
        m8010.id=8010L;
        m8010.icon = "pajamas:work-item-requirement";
        m8010.name = "单一主表案例";
        m8010.ornum = 8010; 
        m8010.comp = "demo/single/main/index";
        m8010.path= "single/main";
        m8010.pid=8000L;
        m8010.catag = false;
        m8010.shtag = true;
        m8010.outag = false;
        list.Add(m8010);
        
        SysPermMenu m8020 = new SysPermMenu();
        m8020.id=8020L;
        m8020.icon = "pajamas:work-item-requirement";
        m8020.name = "单一树表案例";
        m8020.ornum = 8020; 
        m8020.comp = "demo/single/cate/index";
        m8020.path= "single/cate";
        m8020.pid=8000L;
        m8020.catag = false;
        m8020.shtag = true;
        m8020.outag = false;
        list.Add(m8020);
        
        SysPermMenu m8030 = new SysPermMenu();
        m8030.id=8030L;
        m8030.icon = "pajamas:work-item-requirement";
        m8030.name = "关联主分子案例";
        m8030.ornum = 8030; 
        m8030.comp = "demo/link/index";
        m8030.path= "link";
        m8030.pid=8000L;
        m8030.catag = false;
        m8030.shtag = true;
        m8030.outag = false;
        list.Add(m8030);

        foreach (SysPermMenu item in list)
        {
            item.avtag=true;
            if(item.type==null){
                item.type="2";
            }
            item.uptim=item.crtim;
        }
        await _menuRepo.InsertRangeAsync(list);
        
        
        
        
    }

    private async Task initBaseApi()
    {
        List<SysPermApi> list = new List<SysPermApi>();

        SysPermApi a101001 = new SysPermApi();
        a101001.id = 101001L;
        a101001.ornum = 101001;
        a101001.name = "部门查询";
        a101001.menid = 1010L;
        a101001.perm = "sysorg:dept:query";
        list.Add(a101001);

        SysPermApi a101002 = new SysPermApi();
        a101002.id = 101002L;
        a101002.ornum = 101002;
        a101002.name = "部门编辑";
        a101002.menid = 1010L;
        a101002.perm = "sysorg:dept:edit";
        list.Add(a101002);
        
        SysPermApi a101003 = new SysPermApi();
        a101003.id = 101003L;
        a101003.ornum = 101003;
        a101003.name = "部门删除";
        a101003.menid = 1010L;
        a101003.perm = "sysorg:dept:delete";
        list.Add(a101003);
        
        SysPermApi a102001 = new SysPermApi();
        a102001.id = 102001L;
        a102001.ornum = 102001;
        a102001.name = "用户查询";
        a102001.menid = 1020L;
        a102001.perm = "sysorg:user:query";
        list.Add(a102001);
        
        SysPermApi a102002 = new SysPermApi();
        a102002.id = 102002L;
        a102002.ornum = 102002;
        a102002.name = "用户编辑";
        a102002.menid = 1020L;
        a102002.perm = "sysorg:user:edit";
        list.Add(a102002);
        
        SysPermApi a102003 = new SysPermApi();
        a102003.id = 102003L;
        a102003.ornum = 102003;
        a102003.name = "用户删除";
        a102003.menid = 1020L;
        a102003.perm = "sysorg:user:delete";
        list.Add(a102003);
        
        SysPermApi a102004 = new SysPermApi();
        a102004.id = 102004L;
        a102004.ornum = 102004;
        a102004.name = "用户启用禁用";
        a102004.menid = 1020L;
        a102004.perm = "sysorg:user:avtag";
        list.Add(a102004);
        
        SysPermApi a102005 = new SysPermApi();
        a102005.id = 102005L;
        a102005.ornum = 102005;
        a102005.name = "用户密码修改";
        a102005.menid = 1020L;
        a102005.perm = "sysorg:user:pacod";
        list.Add(a102005);
        
        SysPermApi a103001 = new SysPermApi();
        a103001.id = 103001L;
        a103001.ornum = 103001;
        a103001.name = "岗位查询";
        a103001.menid = 1030L;
        a103001.perm = "sysorg:post:query";
        list.Add(a103001);
        
        SysPermApi a103002 = new SysPermApi();
        a103002.id = 103002L;
        a103002.ornum = 103002;
        a103002.name = "岗位编辑";
        a103002.menid = 1030L;
        a103002.perm = "sysorg:post:edit";
        list.Add(a103002);
        
        SysPermApi a103003 = new SysPermApi();
        a103003.id = 103003L;
        a103003.ornum = 103003;
        a103003.name = "岗位删除";
        a103003.menid = 1030L;
        a103003.perm = "sysorg:post:delete";
        list.Add(a103003);
        
        SysPermApi a104001 = new SysPermApi();
        a104001.id = 104001L;
        a104001.ornum = 104001;
        a104001.name = "群组查询";
        a104001.menid = 1040L;
        a104001.perm = "sysorg:group:query";
        list.Add(a104001);
        
        SysPermApi a104002 = new SysPermApi();
        a104002.id = 104002L;
        a104002.ornum = 104002;
        a104002.name = "群组编辑";
        a104002.menid = 1040L;
        a104002.perm = "sysorg:group:edit";
        list.Add(a104002);
        
        SysPermApi a104003 = new SysPermApi();
        a104003.id = 104003L;
        a104003.ornum = 104003;
        a104003.name = "群组删除";
        a104003.menid = 1040L;
        a104003.perm = "sysorg:group:delete";
        list.Add(a104003);
        
        SysPermApi a104004 = new SysPermApi();
        a104004.id = 104004L;
        a104004.ornum = 104004;
        a104004.name = "群组分类查询";
        a104004.menid = 1040L;
        a104004.perm = "sysorg:groupc:query";
        list.Add(a104004);
        
        SysPermApi a104005 = new SysPermApi();
        a104005.id = 104005L;
        a104005.ornum = 104005;
        a104005.name = "群组分类编辑";
        a104005.menid = 1040L;
        a104005.perm = "sysorg:groupc:edit";
        list.Add(a104005);
        
        SysPermApi a104006 = new SysPermApi();
        a104006.id = 104006L;
        a104006.ornum = 104006;
        a104006.name = "群组分类删除";
        a104006.menid = 1040L;
        a104006.perm = "sysorg:groupc:delete";
        list.Add(a104006);
        
        SysPermApi a105001 = new SysPermApi();
        a105001.id = 105001L;
        a105001.ornum = 105001;
        a105001.name = "菜单查询";
        a105001.menid = 1050L;
        a105001.perm = "sysperm:menu:query";
        list.Add(a105001);
        
        SysPermApi a105002 = new SysPermApi();
        a105002.id = 105002L;
        a105002.ornum = 105002;
        a105002.name = "菜单编辑";
        a105002.menid = 1050L;
        a105002.perm = "sysperm:menu:edit";
        list.Add(a105002);
        
        SysPermApi a105003 = new SysPermApi();
        a105003.id = 105003L;
        a105003.ornum = 105003;
        a105003.name = "菜单删除";
        a105003.menid = 1050L;
        a105003.perm = "sysperm:menu:delete";
        list.Add(a105003);
        
        SysPermApi a106001 = new SysPermApi();
        a106001.id = 106001L;
        a106001.ornum = 106001;
        a106001.name = "接口查询";
        a106001.menid = 1060L;
        a106001.perm = "sysperm:api:query";
        list.Add(a106001);
        
        SysPermApi a106002 = new SysPermApi();
        a106002.id = 106002L;
        a106002.ornum = 106002;
        a106002.name = "接口编辑";
        a106002.menid = 1060L;
        a106002.perm = "sysperm:api:edit";
        list.Add(a106002);
        
        SysPermApi a106003 = new SysPermApi();
        a106003.id = 106003L;
        a106003.ornum = 106003;
        a106003.name = "接口删除";
        a106003.menid = 1060L;
        a106003.perm = "sysperm:api:delete";
        list.Add(a106003);
        
        SysPermApi a107001 = new SysPermApi();
        a107001.id = 107001L;
        a107001.ornum = 107001;
        a107001.name = "角色查询";
        a107001.menid = 1070L;
        a107001.perm = "sysperm:role:query";
        list.Add(a107001);
        
        SysPermApi a107002 = new SysPermApi();
        a107002.id = 107002L;
        a107002.ornum = 107002;
        a107002.name = "角色编辑";
        a107002.menid = 1070L;
        a107002.perm = "sysperm:role:edit";
        list.Add(a107002);
        
        SysPermApi a107003 = new SysPermApi();
        a107003.id = 107003L;
        a107003.ornum = 107003;
        a107003.name = "角色删除";
        a107003.menid = 1070L;
        a107003.perm = "sysperm:role:delete";
        list.Add(a107003);
        
        SysPermApi a108001 = new SysPermApi();
        a108001.id = 108001L;
        a108001.ornum = 108001;
        a108001.name = "参数查询";
        a108001.menid = 1080L;
        a108001.perm = "sys:config:query";
        list.Add(a108001);
        
        SysPermApi a108002 = new SysPermApi();
        a108002.id = 108002L;
        a108002.ornum = 108002;
        a108002.name = "参数编辑";
        a108002.menid = 1080L;
        a108002.perm = "sys:config:edit";
        list.Add(a108002);
        
        SysPermApi a108003 = new SysPermApi();
        a108003.id = 108003L;
        a108003.ornum = 108003;
        a108003.name = "参数删除";
        a108003.menid = 1080L;
        a108003.perm = "sys:config:delete";
        list.Add(a108003);
        
        SysPermApi a109001 = new SysPermApi();
        a109001.id = 109001L;
        a109001.ornum = 109001;
        a109001.name = "通知查询";
        a109001.menid = 1090L;
        a109001.perm = "sys:notice:query";
        list.Add(a109001);
        
        SysPermApi a109002 = new SysPermApi();
        a109002.id = 109002L;
        a109002.ornum = 109002;
        a109002.name = "通知编辑";
        a109002.menid = 1090L;
        a109002.perm = "sys:notice:edit";
        list.Add(a109002);
        
        SysPermApi a109003 = new SysPermApi();
        a109003.id = 109003L;
        a109003.ornum = 109003;
        a109003.name = "通知删除";
        a109003.menid = 1090L;
        a109003.perm = "sys:notice:delete";
        list.Add(a109003);
        
        SysPermApi a201001 = new SysPermApi();
        a201001.id = 201001L;
        a201001.ornum = 201001;
        a201001.name = "在线用户查询";
        a201001.menid = 2010L;
        a201001.perm = "mon:online:query";
        list.Add(a201001);
        
        SysPermApi a201002 = new SysPermApi();
        a201002.id = 201002L;
        a201002.ornum = 201002;
        a201002.name = "在线用户强退";
        a201002.menid = 2010L;
        a201002.perm = "mon:online:delete";
        list.Add(a201002);
        
        SysPermApi a202001 = new SysPermApi();
        a202001.id = 202001L;
        a202001.ornum = 202001;
        a202001.name = "登录日志查询";
        a202001.menid = 2020L;
        a202001.perm = "mon:login:query";
        list.Add(a202001);
        
        SysPermApi a202002 = new SysPermApi();
        a202002.id = 202002L;
        a202002.ornum = 202002;
        a202002.name = "登录日志删除";
        a202002.menid = 2020L;
        a202002.perm = "mon:login:delete";
        list.Add(a202002);
        
        SysPermApi a203001 = new SysPermApi();
        a203001.id = 203001L;
        a203001.ornum = 203001;
        a203001.name = "操作日志查询";
        a203001.menid = 2030L;
        a203001.perm = "mon:oper:query";
        list.Add(a203001);
        
        SysPermApi a203002 = new SysPermApi();
        a203002.id = 203002L;
        a203002.ornum = 203002;
        a203002.name = "操作日志删除";
        a203002.menid = 2030L;
        a203002.perm = "mon:oper:delete";
        list.Add(a203002);
        
        SysPermApi a204001 = new SysPermApi();
        a204001.id = 204001L;
        a204001.ornum = 204001;
        a204001.name = "服务器信息查询";
        a204001.menid = 2040L;
        a204001.perm = "mon:server:query";
        list.Add(a204001);
        
        SysPermApi a205001 = new SysPermApi();
        a205001.id = 205001L;
        a205001.ornum = 205001;
        a205001.name = "缓存信息查询";
        a205001.menid = 2050L;
        a205001.perm = "mon:cache:query";
        list.Add(a205001);
        
        SysPermApi a206001 = new SysPermApi();
        a206001.id = 206001L;
        a206001.ornum = 206001;
        a206001.name = "定时任务查询";
        a206001.menid = 2060L;
        a206001.perm = "monjob:main:query";
        list.Add(a206001);
        
        SysPermApi a206002 = new SysPermApi();
        a206002.id = 206002L;
        a206002.ornum = 206002;
        a206002.name = "定时任务修改";
        a206002.menid = 2060L;
        a206002.perm = "monjob:main:edit";
        list.Add(a206002);
        
        SysPermApi a206003 = new SysPermApi();
        a206003.id = 206003L;
        a206003.ornum = 206003;
        a206003.name = "定时任务执行";
        a206003.menid = 2060L;
        a206003.perm = "monjob:main:run";
        list.Add(a206003);
        
        SysPermApi a206101 = new SysPermApi();
        a206101.id = 206101L;
        a206101.ornum = 206101;
        a206101.name = "定时任务日志查询";
        a206101.menid = 2061L;
        a206101.perm = "monjob:log:query";
        list.Add(a206101);
        
        SysPermApi a206102 = new SysPermApi();
        a206102.id = 206102L;
        a206102.ornum = 206102;
        a206102.name = "定时任务日志删除";
        a206102.menid = 2061L;
        a206102.perm = "monjob:log:delete";
        list.Add(a206102);
        
        SysPermApi a301001 = new SysPermApi();
        a301001.id = 301001L;
        a301001.ornum = 301001;
        a301001.name = "字典查询";
        a301001.menid = 3010L;
        a301001.perm = "tooldict:main:query";
        list.Add(a301001);
        
        SysPermApi a301002 = new SysPermApi();
        a301002.id = 301002L;
        a301002.ornum = 301002;
        a301002.name = "字典修改";
        a301002.menid = 3010L;
        a301002.perm = "tooldict:main:edit";
        list.Add(a301002);
        
        SysPermApi a301003 = new SysPermApi();
        a301003.id = 301003L;
        a301003.ornum = 301003;
        a301003.name = "字典删除";
        a301003.menid = 3010L;
        a301003.perm = "tooldict:main:delete";
        list.Add(a301003);
        
        SysPermApi a301004 = new SysPermApi();
        a301004.id = 301004L;
        a301004.ornum = 301004;
        a301004.name = "字典数据查询";
        a301004.menid = 3010L;
        a301004.perm = "tooldict:data:query";
        list.Add(a301004);
        
        SysPermApi a301005 = new SysPermApi();
        a301005.id = 301005L;
        a301005.ornum = 301005;
        a301005.name = "字典数据编辑";
        a301005.menid = 3010L;
        a301005.perm = "tooldict:data:edit";
        list.Add(a301005);
        
        SysPermApi a301006 = new SysPermApi();
        a301006.id = 301006L;
        a301006.ornum = 301006;
        a301006.name = "字典数据编辑";
        a301006.menid = 3010L;
        a301006.perm = "tooldict:data:delete";
        list.Add(a301006);
        
        SysPermApi a302001 = new SysPermApi();
        a302001.id = 302001L;
        a302001.ornum = 302001;
        a302001.name = "编号查询";
        a302001.menid = 3020L;
        a302001.perm = "tool:num:query";
        list.Add(a302001);
        
        SysPermApi a302002 = new SysPermApi();
        a302002.id = 302002L;
        a302002.ornum = 302002;
        a302002.name = "编号编辑";
        a302002.menid = 3020L;
        a302002.perm = "tool:num:edit";
        list.Add(a302002);
        
        SysPermApi a302003 = new SysPermApi();
        a302003.id = 302003L;
        a302003.ornum = 302003;
        a302003.name = "编号删除";
        a302003.menid = 3020L;
        a302003.perm = "tool:num:delete";
        list.Add(a302003);
        
        foreach (SysPermApi item in list)
        {
            item.avtag=true;
            item.uptim=item.crtim;
        }
        await _apiRepo.InsertRangeAsync(list);
        
        
        
        List<SysPermApi> list2 = new List<SysPermApi>();

        SysPermApi a801001 = new SysPermApi();
        a801001.id = 801001L;
        a801001.ornum = 801001;
        a801001.name = "单一主表-查询";
        a801001.menid = 8010L;
        a801001.perm = "single:main:query";
        list2.Add(a801001);

        SysPermApi a801002 = new SysPermApi();
        a801002.id = 801002L;
        a801002.ornum = 801002;
        a801002.name = "单一主表-新增";
        a801002.menid = 8010L;
        a801002.perm = "single:main:add";
        list2.Add(a801002);
        
        SysPermApi a801003 = new SysPermApi();
        a801003.id = 801003L;
        a801003.ornum = 801003;
        a801003.name = "单一主表-修改";
        a801003.menid = 8010L;
        a801003.perm = "single:main:edit";
        list2.Add(a801003);
        
        SysPermApi a801004 = new SysPermApi();
        a801004.id = 801004L;
        a801004.ornum = 801004;
        a801004.name = "单一主表-删除";
        a801004.menid = 8010L;
        a801004.perm = "single:main:remove";
        list2.Add(a801004);
        
        SysPermApi a802001 = new SysPermApi();
        a802001.id = 802001L;
        a802001.ornum = 802001;
        a802001.name = "单一树表-查询";
        a802001.menid = 8020L;
        a802001.perm = "single:cate:query";
        list2.Add(a802001);
        
        SysPermApi a802002 = new SysPermApi();
        a802002.id = 802002L;
        a802002.ornum = 802002;
        a802002.name = "单一树表-新增";
        a802002.menid = 8020L;
        a802002.perm = "single:cate:add";
        list2.Add(a802002);
        
        SysPermApi a802003 = new SysPermApi();
        a802003.id = 802003L;
        a802003.ornum = 802003;
        a802003.name = "单一树表-修改";
        a802003.menid = 8020L;
        a802003.perm = "single:cate:edit";
        list2.Add(a802003);
        
        SysPermApi a802004 = new SysPermApi();
        a802004.id = 802004L;
        a802004.ornum = 802004;
        a802004.name = "单一树表-删除";
        a802004.menid = 8020L;
        a802004.perm = "single:cate:remove";
        list2.Add(a802004);
        
        SysPermApi a803001 = new SysPermApi();
        a803001.id = 803001L;
        a803001.ornum = 803001;
        a803001.name = "关联主表-查询";
        a803001.menid = 8030L;
        a803001.perm = "link:main:query";
        list2.Add(a803001);
        
        SysPermApi a803002 = new SysPermApi();
        a803002.id = 803002L;
        a803002.ornum = 803002;
        a803002.name = "关联主表-新增";
        a803002.menid = 8030L;
        a803002.perm = "link:main:add";
        list2.Add(a803002);
        
        SysPermApi a803003 = new SysPermApi();
        a803003.id = 803003L;
        a803003.ornum = 803003;
        a803003.name = "关联主表-修改";
        a803003.menid = 8030L;
        a803003.perm = "link:main:edit";
        list2.Add(a803003);
        
        SysPermApi a803004 = new SysPermApi();
        a803004.id = 803004L;
        a803004.ornum = 803004;
        a803004.name = "关联主表-删除";
        a803004.menid = 8030L;
        a803004.perm = "link:main:remove";
        list2.Add(a803004);
        
        SysPermApi a803011 = new SysPermApi();
        a803011.id = 803011L;
        a803011.ornum = 803011;
        a803011.name = "关联树表-查询";
        a803011.menid = 8030L;
        a803011.perm = "link:cate:query";
        list2.Add(a803011);
        
        SysPermApi a803012 = new SysPermApi();
        a803012.id = 803012L;
        a803012.ornum = 803012;
        a803012.name = "关联树表-新增";
        a803012.menid = 8030L;
        a803012.perm = "link:cate:add";
        list2.Add(a803012);
        
        SysPermApi a803013 = new SysPermApi();
        a803013.id = 803013L;
        a803013.ornum = 803013;
        a803013.name = "关联树表-修改";
        a803013.menid = 8030L;
        a803013.perm = "link:cate:edit";
        list2.Add(a803013);
        
        SysPermApi a803014 = new SysPermApi();
        a803014.id = 803014L;
        a803014.ornum = 803014;
        a803014.name = "关联树表-删除";
        a803014.menid = 8030L;
        a803014.perm = "link:cate:remove";
        list2.Add(a803014);
        
        foreach (SysPermApi item in list2)
        {
            item.avtag=true;
            item.uptim=item.crtim;
        }
        await _apiRepo.InsertRangeAsync(list2);
    }
    
    private async Task initBpmApi()
    {
        List<SysPermApi> list = new List<SysPermApi>();

        SysPermApi a603001 = new SysPermApi();
        a603001.id = 603001L;
        a603001.ornum = 603001;
        a603001.name = "流程查询";
        a603001.menid = 6030L;
        a603001.perm = "bpmbus:main:query";
        list.Add(a603001);
        
        SysPermApi a603002 = new SysPermApi();
        a603002.id = 603002L;
        a603002.ornum = 603002;
        a603002.name = "流程新增";
        a603002.menid = 6030L;
        a603002.perm = "bpmbus:main:add";
        list.Add(a603002);
        
        SysPermApi a603003 = new SysPermApi();
        a603003.id = 603003L;
        a603003.ornum = 603003;
        a603003.name = "流程编辑";
        a603003.menid = 6030L;
        a603003.perm = "bpmbus:main:edit";
        list.Add(a603003);
        
        foreach (SysPermApi item in list)
        {
            item.avtag=true;
            item.uptim=item.crtim;
        }
        await _apiRepo.InsertRangeAsync(list);
    }
    
    private async Task initRole()
    {
        SysPermRole role = new SysPermRole();
        role.id = 1L;
        role.name = "管理员";
        role.notes = "拥有所有权限";
        role.ornum = 1;
        role.orgs =
        [
            new SysOrg("u2"),
            new SysOrg("u3"),
            new SysOrg("u4"),
            new SysOrg("u5")
        ];
        role.menus = await _menuRepo.GetListAsync();
        role.apis = await _apiRepo.GetListAsync();
        await _roleRepo.Context.InsertNav(role)
            .Include(it => it.orgs)
            .Include(it => it.menus)
            .Include(it => it.apis)
            .ExecuteCommandAsync();
        
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}