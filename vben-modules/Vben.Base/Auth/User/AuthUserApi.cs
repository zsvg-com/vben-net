using Vben.Base.Sys.Org.User;
using Vben.Base.Sys.Perm.Menu;

namespace Vben.Base.Auth.User;

[Route("")]
[ApiDescriptionSettings("Auth", Tag = "用户信息")]
public class AuthUserApi(
    SqlSugarRepository<SysPermMenu> menuRepo,
    SqlSugarRepository<SysOrgUser> userRepo,
    IUserManager userManager)
    : ControllerBase
{


    [HttpGet("system/user/getInfo")]
    // [AllowAnonymous]
    public async Task<UserInfoVo> info()
    {
        if (userManager.UserId == null)
        {
            return null;
        }

        UserInfoVo userInfoVo = new UserInfoVo();
        SysUserVo userVo = new SysUserVo();
        userVo.userId = userManager.UserId;
        userVo.userName = userManager.Account;
        userVo.nickName = userManager.Name;
        userVo.deptName = "XX科技";
        userVo.deptId = userManager.DeptId;
        userVo.avatar = "http://8.153.168.178:8080/tool/oss/main/show?id=1978437156538343424";
        userInfoVo.user = userVo;
        BuildPerms(userInfoVo);
        return userInfoVo;
    }
    
    /**
   * 构建登录用户
   */
    private void BuildPerms(UserInfoVo userInfoVo) {
        if("u1"==userInfoVo.user.userId){
            userInfoVo.permissions = new HashSet<string>();
            userInfoVo.permissions.Add("*:*:*");
            userInfoVo.roles = new HashSet<string>();
            userInfoVo.roles.Add("superadmin");
        }else{
            userInfoVo.permissions = new HashSet<string>();
            string oids= FindConds(userManager.UserId);
            string sql="select distinct perm id from sys_perm_api a inner join sys_perm_role_api ra on ra.aid=a.id inner join sys_perm_role_org ro on ro.rid=ra.rid  where a.avtag="+Db.True+" and ro.oid in ("+oids+")";
            List<string> permArr= userRepo.Context.Ado.SqlQuery<string>(sql);
            userInfoVo.permissions = new HashSet<string>(permArr);
            userInfoVo.roles = new HashSet<string>();
            userInfoVo.roles.Add("superadmin");
        }
    }

    // [HttpGet("/system/user/getInfo")]
    // [AllowAnonymous]
    // public async Task<UserInfoVo> info()
    // {
    //      UserInfoVo userInfoVo = new UserInfoVo();
    //      SysUserVo userVo = new SysUserVo();
    //      userVo.userId = 1;
    //      userVo.userName = "admin";
    //      userVo.nickName = "管理员";
    //      userVo.deptName = "XX科技";
    //      userVo.deptId = 1;
    //      userVo.avatar = "http://8.153.168.178:8080/tool/oss/main/show?id=1978437156538343424";
    //      userInfoVo.user = userVo;
    //      userInfoVo.permissions = new HashSet<string>();
    //      userInfoVo.permissions.Add("*:*:*");
    //      userInfoVo.roles = new HashSet<string>();
    //      userInfoVo.roles.Add("superadmin");
    //      return userInfoVo;
    // }

    [HttpGet("/system/menu/getRouters")]
    public async Task<List<RouterVo>> routers()
    {
        if (userManager.UserId == null)
        {
            throw Oops.Oh("登录过期").StatusCode(401);
        }

        var menuList = new List<SysPermMenu>();
        if (userManager.UserId == "u1")
        {
            menuList = await menuRepo.Context.SqlQueryable<SysPermMenu>
                    ("select m.shtag,m.type,m.name,m.path,m.icon,m.comp,m.ornum,m.id,m.pid,m.outag,m.catag from sys_perm_menu m where m.avtag="+Db.True+" order by m.pid,m.ornum")
                .ToTreeAsync(it => it.children, it => it.pid, 0);
        }
        else
        {
            string oids = FindConds(userManager.UserId);
            string sql =
                "select distinct m.shtag,m.type,m.name,m.path,m.icon,m.comp,m.ornum,m.id,m.pid,m.outag,m.catag from sys_perm_menu m inner join sys_perm_role_menu rm on rm.mid=m.id inner join sys_perm_role_org ro on ro.rid=rm.rid  where m.avtag="+Db.True+" and ro.oid in (" +
                oids + ") order by m.pid,m.ornum";
            menuList = await menuRepo.Context.SqlQueryable<SysPermMenu>(sql)
                .ToTreeAsync(it => it.children, it => it.pid, 0);
        }

        // List<SysPermMenu> list= await _menuRepo.Context.Queryable<SysPermMenu>().ToListAsync();

        // List<RouterVo> routerVoList = new List<RouterVo>();
        List<RouterVo> routerVoList = buildMenus(menuList);
        return routerVoList;
    }

    private List<RouterVo> buildMenus(List<SysPermMenu> menus)
    {
        List<RouterVo> routers = new List<RouterVo>();
        foreach (var menu in menus)
        {
            string name = menu.getRouteName() + menu.id;
            RouterVo router = new RouterVo();
            router.hidden = !menu.shtag;
            router.name = name;
            router.path = menu.pid == 0L ? "/" + menu.path : menu.path;
            router.component = menu.comp;
            router.query = menu.param;
            router.meta = new MetaVo
            {
                title = menu.name,
                icon = menu.icon,
                noCache = !menu.catag,
                link = menu.path,
            };
            List<SysPermMenu> cMenus = menu.children;
            if ("1" == menu.type)
            {
                router.alwaysShow = true;
                router.redirect = "noRedirect";
                router.children = buildMenus(cMenus);
            }
            else if (menu.isMenuFrame())
            {
                string frameName = StrUtils.UpperFirst(menu.path) + menu.id;
                router.meta = null;
                List<RouterVo> childrenList = new List<RouterVo>();
                RouterVo children = new RouterVo();
                children.path = menu.path;
                children.component = menu.comp;
                children.name = frameName;
                children.meta = new MetaVo
                {
                    title = menu.name,
                    icon = menu.icon,
                    noCache = !menu.catag,
                    link = menu.path,
                };
                children.query = menu.param;
                childrenList.Add(children);
                router.children = childrenList;
            }
            else if (menu.pid == 0L && menu.isInnerLink())
            {
                router.meta = new MetaVo
                {
                    title = menu.name,
                    icon = menu.icon,
                };
                router.path = "/";
                List<RouterVo> childrenList = new List<RouterVo>();
                RouterVo children = new RouterVo();
                string routerPath = SysPermMenu.innerLinkReplaceEach(menu.path);
                string innerLinkName = StrUtils.UpperFirst(routerPath) + menu.id;
                children.path = routerPath;
                children.component = "InnerLink";
                children.name = innerLinkName;
                children.meta = new MetaVo
                {
                    title = menu.name,
                    icon = menu.icon,
                    link = menu.path,
                };
                childrenList.Add(children);
                router.children = childrenList;
            }

            routers.Add(router);
        }

        return routers;
    }

    //获取组织架构可用集
    private string FindConds(string id)
    {
        string tierSql = "select tier from sys_org_user where id = @id";
        var tier = userRepo.Context.Ado.SqlQuerySingle<string>(tierSql, new { id });

        StringBuilder conds = new StringBuilder();
        //1. conds拼接父级id
        if (!string.IsNullOrEmpty(tier))
        {
            string[] pidArr = tier.Split("_");
            for (int i = pidArr.Length - 1; i >= 0; i--)
            {
                if ("" != pidArr[i])
                {
                    conds.Append("'").Append(pidArr[i]).Append("',");
                }
            }
        }
        else
        {
            conds = new StringBuilder("'" + id + "',");
        }

        //2. conds拼接岗位id
        List<string> postList = FindPostList(id);
        foreach (var str in postList)
        {
            conds.Append("'").Append(str).Append("',");
        }

        conds = new StringBuilder(conds.ToString().Substring(0, conds.Length - 1)); //优化
        //3. conds拼接群组id
        List<string> groupList = FindGroupList(conds.ToString());
        foreach (var str in groupList)
        {
            conds.Append(",'").Append(str).Append("'");
        }

        return conds.ToString();
    }

    //获取组织架构岗位id集合
    private List<string> FindPostList(string oid)
    {
        string sql = "select pid as id from sys_org_post_org where oid=@oid";
        return userRepo.Context.Ado.SqlQuery<string>(sql, new { oid });
    }

    //获取组织架构群组id集合
    private List<string> FindGroupList(string conds)
    {
        string sql = "select DISTINCT gid as id from sys_org_group_org where oid in (" + conds + ")";
        return userRepo.Context.Ado.SqlQuery<string>(sql);
    }
}