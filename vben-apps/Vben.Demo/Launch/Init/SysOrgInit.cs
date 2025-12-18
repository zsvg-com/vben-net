using Vben.Base.Sys.Org.Dept;
using Vben.Base.Sys.Org.Group;
using Vben.Base.Sys.Org.Post;
using Vben.Base.Sys.Org.User;
using Vben.Common.Sqlsugar.Config;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Admin.Launch.Init;

public class SysOrgInit : ITransient
{
    private readonly SqlSugarRepository<SysOrgDept> _deptRepo;
    private readonly SqlSugarRepository<SysOrg> _orgRepo;
    private readonly SqlSugarRepository<SysOrgUser> _userRepo;
    private readonly SqlSugarRepository<SysOrgPost> _postRepo;
    private readonly SqlSugarRepository<SysOrgGroup> _groupRepo;
    private string rootId = "d1000";

    public SysOrgInit(SqlSugarRepository<SysOrgDept> deptRepo,
        SqlSugarRepository<SysOrg> orgRepo,
        SqlSugarRepository<SysOrgUser> userRepo,
        SqlSugarRepository<SysOrgPost> postRepo,
        SqlSugarRepository<SysOrgGroup> groupRepo)
    {
        _deptRepo = deptRepo;
        _orgRepo = orgRepo;
        _userRepo = userRepo;
        _postRepo = postRepo;
        _groupRepo = groupRepo;
    }

    public async Task initData()
    {
        await initDept();
        await initUser();
        await initPost();
        await initGroup();
    }

    private async Task initDept()
    {
        SysOrgDept dept = new SysOrgDept();
        dept.id = "d1000";
        dept.name = "XX科技";
        dept.type = 1;
        dept.tier = "_d1000_";
        dept.ornum = 1000;
        dept.avtag = true;
        await _deptRepo.InsertAsync(dept);
        await _orgRepo.InsertAsync(new SysOrg(dept.id, dept.name, 1));
        
        SysOrgDept d1100 = new SysOrgDept();
        d1100.id = "d1100";
        d1100.name = "北京分公司";
        d1100.type = 2;
        d1100.pid = "d1000";
        d1100.tier = "_d1000_d1100_";
        d1100.ornum = 1100;
        d1100.avtag = true;
        await _deptRepo.InsertAsync(d1100);
        await _orgRepo.InsertAsync(new SysOrg(d1100.id, d1100.name, 1));
        
        SysOrgDept d1110 = new SysOrgDept();
        d1110.id = "d1110";
        d1110.name = "北京分公司销售部";
        d1110.type = 8;
        d1110.pid = "d1100";
        d1110.tier = "_d1000_d1100_d1110_";
        d1110.ornum = 1110;
        d1110.avtag = true;
        await _deptRepo.InsertAsync(d1110);
        await _orgRepo.InsertAsync(new SysOrg(d1110.id, d1110.name, 1));
        
        SysOrgDept d1111 = new SysOrgDept();
        d1111.id = "d1111";
        d1111.name = "北京分公司销售部一组";
        d1111.type = 8;
        d1111.pid = "d1110";
        d1111.tier = "_d1000_d1100_d1110_d1111_";
        d1111.ornum = 1111;
        d1111.avtag = true;
        await _deptRepo.InsertAsync(d1111);
        await _orgRepo.InsertAsync(new SysOrg(d1111.id, d1111.name, 1));
        
        SysOrgDept d1112 = new SysOrgDept();
        d1112.id = "d1112";
        d1112.name = "北京分公司销售部二组";
        d1112.type = 8;
        d1112.pid = "d1110";
        d1112.tier = "_d1000_d1100_d1110_d1112_";
        d1112.ornum = 1112;
        d1112.avtag = true;
        await _deptRepo.InsertAsync(d1112);
        await _orgRepo.InsertAsync(new SysOrg(d1112.id, d1112.name, 1));
        
        SysOrgDept d1120 = new SysOrgDept();
        d1120.id = "d1120";
        d1120.name = "北京分公司人事部";
        d1120.type = 8;
        d1120.pid = "d1100";
        d1120.tier = "_d1000_d1100_d1120_";
        d1120.ornum = 1120;
        d1120.avtag = true;
        await _deptRepo.InsertAsync(d1120);
        await _orgRepo.InsertAsync(new SysOrg(d1120.id, d1120.name, 1));
        
        SysOrgDept d1130 = new SysOrgDept();
        d1130.id = "d1130";
        d1130.name = "北京分公司财务部";
        d1130.type = 8;
        d1130.pid = "d1100";
        d1130.tier = "_d1000_d1100_d1130_";
        d1130.ornum = 1130;
        d1130.avtag = true;
        await _deptRepo.InsertAsync(d1130);
        await _orgRepo.InsertAsync(new SysOrg(d1130.id, d1130.name, 1));
        
        SysOrgDept d1140 = new SysOrgDept();
        d1140.id = "d1140";
        d1140.name = "北京分公司综合部";
        d1140.type = 8;
        d1140.pid = "d1100";
        d1140.tier = "_d1000_d1100_d1140_";
        d1140.ornum = 1140;
        d1140.avtag = true;
        await _deptRepo.InsertAsync(d1140);
        await _orgRepo.InsertAsync(new SysOrg(d1140.id, d1140.name, 1));
        
        SysOrgDept d1200 = new SysOrgDept();
        d1200.id = "d1200";
        d1200.name = "上海分公司";
        d1200.type = 2;
        d1200.pid = "d1000";
        d1200.tier = "_d1000_d1200_";
        d1200.ornum = 1200;
        d1200.avtag = true;
        await _deptRepo.InsertAsync(d1200);
        await _orgRepo.InsertAsync(new SysOrg(d1200.id, d1200.name, 1));
        
        SysOrgDept d1210 = new SysOrgDept();
        d1210.id = "d1210";
        d1210.name = "上海分公司销售部";
        d1210.type = 8;
        d1210.pid = "d1200";
        d1210.tier = "_d1000_d1200_d1210_";
        d1210.ornum = 1210;
        d1210.avtag = true;
        await _deptRepo.InsertAsync(d1210);
        await _orgRepo.InsertAsync(new SysOrg(d1210.id, d1210.name, 1));
        
        SysOrgDept d1220 = new SysOrgDept();
        d1220.id = "d1220";
        d1220.name = "上海分公司人事部";
        d1220.type = 8;
        d1220.pid = "d1200";
        d1220.tier = "_d1000_d1200_d1220_";
        d1220.ornum = 1220;
        d1220.avtag = true;
        await _deptRepo.InsertAsync(d1220);
        await _orgRepo.InsertAsync(new SysOrg(d1220.id, d1220.name, 1));
        
        SysOrgDept d1230 = new SysOrgDept();
        d1230.id = "d1230";
        d1230.name = "上海分公司财务部";
        d1230.type = 8;
        d1230.pid = "d1200";
        d1230.tier = "_d1000_d1200_d1230_";
        d1230.ornum = 1230;
        d1230.avtag = true;
        await _deptRepo.InsertAsync(d1230);
        await _orgRepo.InsertAsync(new SysOrg(d1230.id, d1230.name, 1));
        
        SysOrgDept d1300 = new SysOrgDept();
        d1300.id = "d1300";
        d1300.name = "广州分公司";
        d1300.type = 2;
        d1300.pid = "d1000";
        d1300.tier = "_d1000_d1300_";
        d1300.ornum = 1300;
        d1300.avtag = true;
        await _deptRepo.InsertAsync(d1300);
        await _orgRepo.InsertAsync(new SysOrg(d1300.id, d1300.name, 1));
        
        SysOrgDept d1310 = new SysOrgDept();
        d1310.id = "d1310";
        d1310.name = "广州分公司综合部";
        d1310.type = 8;
        d1310.pid = "d1300";
        d1310.tier = "_d1000_d1300_d1310_";
        d1310.ornum = 1310;
        d1310.avtag = true;
        await _deptRepo.InsertAsync(d1310);
        await _orgRepo.InsertAsync(new SysOrg(d1310.id, d1310.name, 1));
        
        SysOrgDept d1320 = new SysOrgDept();
        d1320.id = "d1320";
        d1320.name = "广州分公司销售部";
        d1320.type = 8;
        d1320.pid = "d1300";
        d1320.tier = "_d1000_d1300_d1320_";
        d1320.ornum = 1320;
        d1320.avtag = true;
        await _deptRepo.InsertAsync(d1320);
        await _orgRepo.InsertAsync(new SysOrg(d1320.id, d1320.name, 1));
        
        SysOrgDept d1330 = new SysOrgDept();
        d1330.id = "d1330";
        d1330.name = "广州分公司人事部";
        d1330.type = 8;
        d1330.pid = "d1300";
        d1330.tier = "_d1000_d1300_d1330_";
        d1330.ornum = 1330;
        d1330.avtag = true;
        await _deptRepo.InsertAsync(d1330);
        await _orgRepo.InsertAsync(new SysOrg(d1330.id, d1330.name, 1));
    }

    private async Task initUser()
    {
        SysOrgUser u1 = new SysOrgUser();
        u1.id = "u1";
        u1.name = "管理员";
        u1.ninam = "管理员";
        u1.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u1.usnam = "admin";
        u1.depid = "d1000";
        u1.tier = "_d1000_u1_";
        u1.monum = "13812345678";
        u1.email = "admin@qq.com";
        u1.sex = "1";
        u1.notes = "管理员不给修改";
        u1.ornum = 1;
        u1.avtag = true;
        await _userRepo.InsertAsync(u1);
        await _orgRepo.InsertAsync(new SysOrg(u1.id, u1.name, 2));
        
        SysOrgUser u2 = new SysOrgUser();
        u2.id = "u2";
        u2.name = "小狐狸";
        u2.ninam = "小狐狸";
        u2.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u2.usnam = "vben";
        u2.depid = "d1000";
        u2.tier = "_d1000_u2_";
        u2.monum = "13912345678";
        u2.email = "vben@qq.com";
        u2.sex = "2";
        u2.ornum = 2;
        u2.avtag = true;
        await _userRepo.InsertAsync(u2);
        await _orgRepo.InsertAsync(new SysOrg(u2.id, u2.name, 2));
        
        SysOrgUser u3 = new SysOrgUser();
        u3.id = "u3";
        u3.name = "张三";
        u3.ninam = "张三";
        u3.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u3.usnam = "zs";
        u3.depid = "d1000";
        u3.tier = "_d1000_u3_";
        u3.ornum = 3;
        u3.avtag = true;
        await _userRepo.InsertAsync(u3);
        await _orgRepo.InsertAsync(new SysOrg(u3.id, u3.name, 2));
        
        SysOrgUser u4 = new SysOrgUser();
        u4.id = "u4";
        u4.name = "李四";
        u4.ninam = "李四";
        u4.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u4.usnam = "ls";
        u4.depid = "d1100";
        u4.tier = "_d1000_d1100_u4_";
        u4.ornum = 4;
        u4.avtag = true;
        await _userRepo.InsertAsync(u4);
        await _orgRepo.InsertAsync(new SysOrg(u4.id, u4.name, 2));
        
        SysOrgUser u5 = new SysOrgUser();
        u5.id = "u5";
        u5.name = "王五";
        u5.ninam = "王五";
        u5.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u5.usnam = "ww";
        u5.depid = "d1110";
        u5.tier = "_d1000_d1100_d1110_u5_";
        u5.ornum = 5;
        u5.avtag = true;
        await _userRepo.InsertAsync(u5);
        await _orgRepo.InsertAsync(new SysOrg(u5.id, u5.name, 2));
        
        SysOrgUser u6 = new SysOrgUser();
        u6.id = "u6";
        u6.name = "赵六";
        u6.ninam = "赵六";
        u6.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u6.usnam = "zl";
        u6.depid = "d1111";
        u6.tier = "_d1000_d1100_d1110_d1111_u6_";
        u6.ornum = 6;
        u6.avtag = true;
        await _userRepo.InsertAsync(u6);
        await _orgRepo.InsertAsync(new SysOrg(u6.id, u6.name, 2));
        
                
        SysOrgUser u7 = new SysOrgUser();
        u7.id = "u7";
        u7.name = "孙七";
        u7.ninam = "孙七";
        u7.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u7.usnam = "sq";
        u7.depid = "d1111";
        u7.tier = "_d1000_d1100_d1110_d1111_u7_";
        u7.ornum = 7;
        u7.avtag = true;
        await _userRepo.InsertAsync(u7);
        await _orgRepo.InsertAsync(new SysOrg(u7.id, u7.name, 2));
        
        SysOrgUser u8 = new SysOrgUser();
        u8.id = "u8";
        u8.name = "周八";
        u8.ninam = "周八";
        u8.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u8.usnam = "zb";
        u8.depid = "d1111";
        u8.tier = "_d1000_d1100_d1110_d1111_u8_";
        u8.ornum = 8;
        u8.avtag = true;
        await _userRepo.InsertAsync(u8);
        await _orgRepo.InsertAsync(new SysOrg(u8.id, u8.name, 2));
        
        SysOrgUser u9 = new SysOrgUser();
        u9.id = "u9";
        u9.name = "吴九";
        u9.ninam = "吴九";
        u9.pacod = "$2a$10$09f8rxsX4tbj1CZla2MSOuiwHwp5QAPUzbp5whnoZEFK4/xplNwZq";
        u9.usnam = "wj";
        u9.depid = "d1111";
        u9.tier = "_d1000_d1100_d1110_d1111_u9_";
        u9.ornum = 9;
        u9.avtag = true;
        await _userRepo.InsertAsync(u9);
        await _orgRepo.InsertAsync(new SysOrg(u9.id, u9.name, 2));
    }

    private async Task initPost()
    {
        SysOrgPost p1 = new SysOrgPost();
        p1.id = "p2001";
        p1.name = "董事长";
        p1.users =
        [
            new SysOrg("u3", "张三", 2)
        ];
        p1.depid = "d1000";
        p1.tier = "_d1000_p2001_";
        p1.ornum = 2001;
        p1.avtag = true;
        await _postRepo.Context.InsertNav(p1).Include(it => it.users).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(p1.id, p1.name, 4));
        
        SysOrgPost p2 = new SysOrgPost();
        p2.id = "p2002";
        p2.name = "北京分公司总经理";
        p2.users =
        [
            new SysOrg("u4", "李四", 2)
        ];
        p2.depid = "d1100";
        p2.tier = "_d1000_d1100_p2002_";
        p2.ornum = 2002;
        p2.avtag = true;
        await _postRepo.Context.InsertNav(p2).Include(it => it.users).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(p2.id, p2.name, 4));
        
        SysOrgPost p3 = new SysOrgPost();
        p3.id = "p2003";
        p3.name = "北京分公司销售部长";
        p3.users =
        [
            new SysOrg("u5", "王五", 2)
        ];
        p3.depid = "d1110";
        p3.tier = "_d1000_d1100_d1110_p2003_";
        p3.ornum = 2003;
        p3.avtag = true;
        await _postRepo.Context.InsertNav(p3).Include(it => it.users).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(p3.id, p3.name, 4));
        
        SysOrgPost p4 = new SysOrgPost();
        p4.id = "p2004";
        p4.name = "北京分公司销售经理";
        p4.users =
        [
            new SysOrg("u6", "赵六", 2),
            new SysOrg("u7", "孙七", 2)
        ];
        p4.depid = "d1111";
        p4.tier = "_d1000_d1100_d1110_d1111_p2004_";
        p4.ornum = 2004;
        p4.avtag = true;
        await _postRepo.Context.InsertNav(p4).Include(it => it.users).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(p4.id, p4.name, 4));
    }

    private async Task initGroup()
    {
        SysOrgGroup g1 = new SysOrgGroup();
        g1.id = "g3001";
        g1.name = "北京分公司管理组";
        g1.members =
        [
            new SysOrg("u4", "李四", 2),
            new SysOrg("u5", "王五", 2),
            new SysOrg("p2004", "北京分公司销售经理", 4),
            new SysOrg("d1140", "北京分公司综合部", 1)
        ];
        g1.ornum = 3001;
        g1.avtag = true;
        await _groupRepo.Context.InsertNav(g1).Include(it => it.members).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(g1.id, g1.name, 8));
        
        SysOrgGroup g2 = new SysOrgGroup();
        g2.id = "g3002";
        g2.name = "北京分公司销售员";
        g2.members =
        [
            new SysOrg("u8", "周八", 2),
            new SysOrg("u9", "吴九", 2),
        ];
        g2.ornum = 3002;
        g2.avtag = true;
        await _groupRepo.Context.InsertNav(g2).Include(it => it.members).ExecuteCommandAsync();
        await _orgRepo.InsertAsync(new SysOrg(g2.id, g2.name, 8));
    }
}