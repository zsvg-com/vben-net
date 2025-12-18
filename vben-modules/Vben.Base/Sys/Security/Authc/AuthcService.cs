// using Vben.Base.Sys.Org.User;
// using Vben.Base.Sys.Perm.Api;
// using Vben.Base.Sys.Security.Pojo;
// using Vben.Common.Sqlsugar.Config;
// using Vben.Core.Module.Sys;
//
// namespace Vben.Base.Sys.Security.Authc;
//
// public class AuthcService : ITransient
// {
//     private readonly SqlSugarRepository<SysOrgUser> _repo;
//
//     private readonly IJsonSerializerProvider _json;
//
//     public AuthcService(SqlSugarRepository<SysOrgUser> repo, IJsonSerializerProvider jsonSerializer)
//     {
//         _repo = repo;
//         _json = jsonSerializer;
//     }
//
//     public async Task<UserDo> getDbUser(string username)
//     {
//         string sql = @"select t.id,t.name,t.usnam,t.monum,t.pacod,t.relog,t.catag,t.tier,t.type,t.depid,'org' label,d.name depna
//  from sys_org_user t left join sys_org_dept d on d.id=t.depid 
//  where t.usnam=@username and t.avtag="+Db.True;
//         var dbUser = await _repo.Context.Ado.SqlQuerySingleAsync<UserDo>(sql, new { username });
//         if (dbUser == null)
//         {
//             sql = @"select t.id,t.name,t.usnam,t.monum,t.pacod,t.relog,t.catag,t.tier,t.type,t.corid depid,c.catid label,c.name depna from sys_coop_user t 
// left join sys_coop_corp c on c.id=t.corid where t.usnam=@username and t.avtag="+Db.True;
//             dbUser = await _repo.Context.Ado.SqlQuerySingleAsync<UserDo>(sql, new { username });
//             if (dbUser == null)
//             {
//                 throw Oops.Oh(ErrorCode.D1000);
//             }
//         }
//
//         return dbUser;
//     }
//
//     //初始化用户
//     //查询用户组织架构可用集，门户列表，菜单树，API权限集
//     //查询完后做缓存，下次查询直接通过缓存查询
//     public void InitUser(Zuser zuser, UserDo duser, Dictionary<string, object> backDict)
//     {
//         //判断是否已经初始化过了，如已初始化，则可走缓存路线
//         if (!duser.catag) //数据库按步骤路线
//         {
//             //1.获取用户所有的组织架构集:conds
//             if (duser.label == "org")
//             {
//                 zuser.conds = FindConds(duser);
//             }
//             else
//             {
//                 zuser.conds = FindCoopConds(duser);
//             }
//
//             //2.根据组织架构集conds查询前台菜单集:menus
//             List<Zmenu> menuList;
//             List<Zportal> portalList;
//             if (zuser.IsAdmin)
//             {
//                 portalList = FindAllPortalList();
//                 menuList = FindSysMenuList(portalList[0].id);
//             }
//             else
//             {
//                 portalList = FindPortalList(zuser.conds);
//                 if (portalList != null && portalList.Count > 0)
//                 {
//                     menuList = FindMenuList(zuser.conds, portalList, portalList[0].id);
//                 }
//                 else
//                 {
//                     menuList = FindNoPowerMenuList();
//                 }
//             }
//
//             //3.A 设置zuser的权限集（位与代码方式的权限集）
//             List<String> btnList = getBtnList(zuser);
//
//             //3.B 设置zuser的权限集（传统字符串方式）
//             // List<String> btnList = FindBtnList(zuser.conds);
//             // List<string> permList = new List<string>();
//             // foreach (var menu in menuList)
//             // {
//             //     if (!string.IsNullOrEmpty(menu.perm))
//             //     {
//             //         permList.Add(menu.perm);
//             //     }
//             // }
//             // permList.AddRange(btnList);
//             // zuser.permList = permList;
//
//             //4.设置前台返回数据
//             menuList = BuildByRecursive(menuList);
//             backDict.Add("portals", portalList);
//             backDict.Add("menus", menuList);
//             backDict.Add("btns", btnList);
//             backDict.Add("zuser", zuser);
//
//             //5.更新用户，序列化保存数据，使下次这些数据可直接从数据库取
//             if (duser.label == "org")
//             {
//                 updateOrgUserCache(zuser, menuList, btnList, portalList);
//             }
//             else
//             {
//                 updateCoopUserCache(zuser, menuList, btnList, portalList);
//             }
//             // Console.WriteLine("通过数据库");
//         }
//         else //缓存路线，可扩展成redis方式
//         {
//             dynamic cache = null;
//             if (duser.label == "org")
//             {
//                 cache = _repo.Context.Queryable<SysOrgUserCache>().First(it => it.id == zuser.id);
//             }
//             else
//             {
//                 cache = _repo.Context.Queryable<SysCoopUserCache>().First(it => it.id == zuser.id);
//             }
//             zuser.conds = cache.conds;
//             List<Zmenu> menuList = _json.Deserialize<List<Zmenu>>(cache.menus);
//             List<Zportal> portalList = _json.Deserialize<List<Zportal>>(cache.portals);
//             string[] btnArr = null;
//             string btns = cache.btns;
//             if (btns != null)
//             {
//                 btnArr = btns.Split(";");
//             }
//
//             // 设置zuser的权限集（位与代码方式的权限集）
//             zuser.perms = cache.perms;
//
//             // 设置zuser的权限集（传统字符串方式）
//             // string[] permArr = null;
//             // string perms = cache.perms;
//             // if (perms != null)
//             // {
//             //     permArr = perms.Split(";");
//             // }
//             // List<string> permList = new List<string>();
//             // if (permArr != null)
//             // {
//             //     permList = permArr.ToList();
//             // }
//             // zuser.permList = permList;
//
//             backDict.Add("portals", portalList);
//             backDict.Add("menus", menuList);
//             backDict.Add("btns", btnArr);
//             backDict.Add("zuser", zuser);
//             // Console.WriteLine("通过缓存");
//         }
//     }
//
//     //切换门户，根据门户id，设置菜单
//     public async Task SwitchPortal(Zuser zuser, Dictionary<String, Object> backDict, string porid)
//     {
//         //1.根据组织架构集conds查询前台菜单集:menus
//         string sql = "select conds from sys_org_user_cache where id=@id";
//         zuser.conds = await _repo.Context.Ado.SqlQuerySingleAsync<string>(sql, new { id = zuser.id });
//         List<Zmenu> menuList;
//         List<Zportal> portalList;
//         if (zuser.IsAdmin)
//         {
//             portalList = FindAllPortalList();
//             menuList = FindSysMenuList(porid);
//         }
//         else
//         {
//             portalList = FindPortalList(zuser.conds);
//             if (portalList != null && portalList.Count > 0)
//             {
//                 menuList = FindMenuList(zuser.conds, portalList, porid);
//             }
//             else
//             {
//                 menuList = FindNoPowerMenuList();
//             }
//         }
//
//         //2.设置前台返回数据
//         menuList = BuildByRecursive(menuList);
//         backDict.Add("portals", portalList);
//         backDict.Add("menus", menuList);
//     }
//
//     //--------------------------------------组织架构相关逻辑-------------------------------------------
//     //获取组织架构可用集
//     private string FindConds(UserDo duser)
//     {
//         StringBuilder conds = new StringBuilder();
//         //1. conds拼接父级id
//         if (!string.IsNullOrEmpty(duser.tier))
//         {
//             string[] pidArr = duser.tier.Split("_");
//             for (int i = pidArr.Length - 1; i >= 0; i--)
//             {
//                 if ("" != pidArr[i])
//                 {
//                     conds.Append("'").Append(pidArr[i]).Append("',");
//                 }
//             }
//         }
//         else
//         {
//             conds = new StringBuilder("'" + duser.id + "',");
//         }
//
//         //2. conds拼接岗位id
//         List<string> postList = FindPostList(duser.id);
//         foreach (var str in postList)
//         {
//             conds.Append("'").Append(str).Append("',");
//         }
//
//         conds = new StringBuilder(conds.ToString().Substring(0, conds.Length - 1)); //优化
//         //3. conds拼接群组id
//         List<string> groupList = FindGroupList(conds.ToString());
//         foreach (var str in groupList)
//         {
//             conds.Append(",'").Append(str).Append("'");
//         }
//
//         return conds.ToString();
//     }
//
//     //获取组织架构岗位id集合
//     private List<string> FindPostList(string oid)
//     {
//         string sql = "select pid as id from sys_org_post_org where oid=@oid";
//         return _repo.Context.Ado.SqlQuery<string>(sql, new { oid });
//     }
//
//     //获取组织架构群组id集合
//     private List<string> FindGroupList(string conds)
//     {
//         string sql = "select DISTINCT gid as id from sys_org_group_org where oid in (" + conds + ")";
//         return _repo.Context.Ado.SqlQuery<string>(sql);
//     }
//
//     private string FindCoopConds(UserDo duser)
//     {
//         StringBuilder conds = new StringBuilder();
//         //1. conds拼接父级id
//         if (!string.IsNullOrEmpty(duser.tier))
//         {
//             string[] pidArr = duser.tier.Split("x");
//             for (int i = pidArr.Length - 1; i >= 0; i--)
//             {
//                 if ("" != pidArr[i])
//                 {
//                     conds.Append("'").Append(pidArr[i]).Append("',");
//                 }
//             }
//         }
//         else
//         {
//             conds = new StringBuilder("'" + duser.id + "',");
//         }
//         conds = new StringBuilder(conds.ToString().Substring(0, conds.Length - 1)); //优化
//         return conds.ToString();
//     }
//
//
//     //--------------------------------------门户与菜单相关逻辑-------------------------------------------
//     //获取门户集合
//     public List<Zportal> FindPortalList(string conds)
//     {
//         string sql = "select distinct m.name,m.id,m.ornum from sys_portal_main m inner join sys_portal_role r on r.porid=m.id" +
//                      " inner join sys_portal_role_org ro on ro.rid=r.id where m.avtag="+Db.True+" and ro.oid in (" + conds +
//                      ") order by m.ornum";
//         return _repo.Context.Ado.SqlQuery<Zportal>(sql);
//     }
//
//     //获取菜单集合
//     public List<Zmenu> FindMenuList(string conds, List<Zportal> portalList, string porid)
//     {
//         String portals = "";
//         foreach (var zportal in portalList)
//         {
//             portals += "'" + zportal.id + "',";
//         }
//
//         portals = portals.Substring(0, portals.Length - 1);
//
//         //1.获取目录集合
//         string direSql =
//             "select m.porid \"porid\",m.shtag \"shtag\",m.name \"name\",m.code \"code\",m.path \"path\",m.icon \"icon\"," +
//             "m.comp \"comp\",m.ornum \"ornum\",m.id \"id\",m.pid \"pid\",m.perm \"perm\" " +
//             "from sys_portal_menu m where m.type = 'D' and m.porid in (" +
//             portals + ") and m.avtag="+Db.True+" order by m.ornum";
//         List<dynamic> dictList = _repo.Context.Ado.SqlQuery<dynamic>(direSql);
//
//         List<Zmenu> direList = new List<Zmenu>();
//         foreach (var dict in dictList)
//         {
//             Zmenu zmenu = new Zmenu();
//             zmenu.type = "D";
//             zmenu.id = dict.id;
//             zmenu.porid = dict.porid;
//             zmenu.pid = dict.pid;
//             zmenu.perm = dict.perm;
//
//             zmenu.name = dict.code;
//             zmenu.path = dict.path;
//             zmenu.component = dict.comp;
//             Zmeta zmeta = new Zmeta();
//             zmeta.isHide = dict.shtag + "" != "True" && dict.shtag + "" != "1";
//             if (porid != dict.porid)
//             {
//                 zmeta.isHide = true;
//             }
//             zmeta.title = dict.name;
//             zmeta.orderNo = Convert.ToInt32(dict.ornum);
//             zmeta.icon = dict.icon;
//             zmenu.meta = zmeta;
//             direList.Add(zmenu);
//         }
//
//         //2.获取菜单集合
//         string sql =
//             "select distinct m.iftag \"iftag\",m.extag \"extag\",m.porid \"porid\",m.shtag \"shtag\",m.type \"type\",m.name \"name\",m.code \"code\"," +
//             "m.path \"path\",m.icon \"icon\",m.comp \"comp\",m.ornum \"ornum\",m.id \"id\",m.pid \"pid\",m.perm \"perm\",m.catag \"catag\" " +
//             "from sys_portal_menu m inner join sys_portal_role_menu rm on rm.mid=m.id inner join sys_portal_role_org ru on ru.rid=rm.rid where m.type = 'M' and m.porid in (" +
//             portals + ") and m.avtag="+Db.True+" and ru.oid in (" + conds + ") order by m.ornum";
//         List<dynamic> menuList = _repo.Context.Ado.SqlQuery<dynamic>(sql);
//
//         List<Zmenu> list = new List<Zmenu>();
//         foreach (var menu in menuList)
//         {
//             Zmenu zmenu = new Zmenu();
//             zmenu.type = "M";
//             zmenu.id = menu.id;
//             zmenu.porid = menu.porid;
//             zmenu.pid = menu.pid;
//             zmenu.perm = menu.perm;
//
//             zmenu.name = menu.code;
//             zmenu.path = menu.path;
//             zmenu.component = menu.comp;
//             Zmeta zmeta = new Zmeta();
//             zmeta.isHide = menu.shtag + "" != "True" && menu.shtag + "" != "1";
//             // zmeta.isHide = !dict.shtag;
//             if (porid != menu.porid)
//             {
//                 zmeta.isHide = true;
//             }
//             zmeta.title = menu.name;
//             zmeta.orderNo = Convert.ToInt32(menu.ornum);
//             zmeta.icon = menu.icon;
//             zmeta.isKeepAlive = menu.catag + "" == "True" || menu.catag + "" == "1";
//             zmenu.meta = zmeta;
//             bool extag = menu.extag + "" == "True" || menu.extag + "" == "1";
//             if (extag)
//             {
//                 zmeta.isLink = menu.comp;
//                 zmenu.component = "layout/routerView/link";
//             }
//             bool iftag = menu.iftag + "" == "True" || menu.iftag + "" == "1";
//             if (iftag)
//             {
//                 zmeta.isLink = menu.comp;
//                 zmeta.isIframe = true;
//                 zmenu.component = "layout/routerView/iframes";
//             }
//             list.Add(zmenu);
//         }
//
//         //3.将菜单装载到目录
//         List<Zmenu> shouldAddlist1 = new List<Zmenu>();
//         foreach (var zdire in direList)
//         {
//             foreach (var zmenu in list)
//             {
//                 if (zmenu.pid == zdire.id)
//                 {
//                     shouldAddlist1.Add(zdire);
//                     break;
//                 }
//             }
//         }
//
//         List<Zmenu> shouldAddlist2 = new List<Zmenu>();
//         foreach (var zdire in direList)
//         {
//             foreach (var zshould in shouldAddlist1)
//             {
//                 if (zshould.pid != null)
//                 {
//                     if (zshould.pid == zdire.id)
//                     {
//                         shouldAddlist2.Add(zdire);
//                         break;
//                     }
//                 }
//             }
//         }
//
//         List<Zmenu> shouldAddlist3 = new List<Zmenu>();
//         foreach (var zdire in direList)
//         {
//             foreach (var zshould in shouldAddlist2)
//             {
//                 if (zshould.pid != null)
//                 {
//                     if (zshould.pid == zdire.id)
//                     {
//                         shouldAddlist3.Add(zdire);
//                         break;
//                     }
//                 }
//             }
//         }
//
//         listAdd(list, shouldAddlist1);
//         listAdd(list, shouldAddlist2);
//         listAdd(list, shouldAddlist3);
//         // list.AddRange(shouldAddlist1);
//         // list.AddRange(shouldAddlist2);
//         // list.AddRange(shouldAddlist3);
//         if (list.Count == 0)
//         {
//             return FindNoPowerMenuList();
//         }
//
//         if (portalList.Count == 1)
//         {
//             return list;
//         }
//
//         //4.如果存在多个门户，则要进一步处理。
//         //去除其他门户相同name或path的菜单
//         List<Zmenu> backList = new List<Zmenu>();
//         foreach (var zmenu in list)
//         {
//             if (porid == zmenu.porid)
//             {
//                 backList.Add(zmenu);
//             }
//         }
//
//         foreach (var zmenu in list)
//         {
//             if (porid != zmenu.porid)
//             {
//                 bool flag = false;
//                 foreach (var backMenu in backList)
//                 {
//                     if (backMenu.name == zmenu.name || backMenu.path == zmenu.path)
//                     {
//                         flag = true;
//                         break;
//                     }
//                 }
//
//                 if (!flag)
//                 {
//                     backList.Add(zmenu);
//                 }
//             }
//         }
//
//         return backList;
//     }
//
//     //获取所有门户列表，管理员用
//     public List<Zportal> FindAllPortalList()
//     {
//         string sql = "select id,name from sys_portal_main where avtag="+Db.True+" order by ornum";
//         return _repo.Context.Ado.SqlQuery<Zportal>(sql);
//     }
//
//     //获取管理员门户的菜单，管理员用
//     public List<Zmenu> FindSysMenuList(String porid)
//     {
//         String sql = "select m.iftag \"iftag\",m.extag \"extag\",m.porid \"porid\",m.shtag \"shtag\",m.type \"type\",m.name \"name\",m.code \"code\"," +
//                      "m.path \"path\",m.icon \"icon\",m.comp \"comp\",m.ornum \"ornum\",m.id \"id\"," +
//                      "m.pid \"pid\",m.perm \"perm\",m.catag \"catag\" " +
//                      "from sys_portal_menu m where  m.avtag="+Db.True+" order by m.ornum";
//
//         List<dynamic> menuList = _repo.Context.Ado.SqlQuery<dynamic>(sql);
//
//         List<Zmenu> list = new List<Zmenu>();
//         foreach (var menu in menuList)
//         {
//             Zmenu zmenu = new Zmenu();
//             zmenu.id = menu.id;
//             zmenu.porid = menu.porid;
//             zmenu.pid = menu.pid;
//             zmenu.perm = menu.perm;
//             zmenu.type = menu.type;
//             zmenu.name = menu.code;
//             zmenu.path = menu.path;
//             zmenu.component = menu.comp;
//             Zmeta zmeta = new Zmeta();
//             zmeta.isHide = menu.shtag + "" != "True" && menu.shtag + "" != "1";
//             if (porid != menu.porid)
//             {
//                 zmeta.isHide = true;
//             }
//
//             zmeta.title = menu.name;
//             zmeta.orderNo = Convert.ToInt32(menu.ornum);
//             zmeta.icon = menu.icon;
//             zmeta.isKeepAlive = menu.catag + "" == "True" || menu.catag + "" == "1";
//             zmenu.meta = zmeta;
//             bool extag = menu.extag + "" == "True" || menu.extag + "" == "1";
//             if (extag)
//             {
//                 zmeta.isLink = menu.comp;
//                 zmenu.component = "layout/routerView/link";
//             }
//             bool iftag = menu.iftag + "" == "True" || menu.iftag + "" == "1";
//             if (iftag)
//             {
//                 zmeta.isLink = menu.comp;
//                 zmeta.isIframe = true;
//                 zmenu.component = "layout/routerView/iframes";
//             }
//             list.Add(zmenu);
//         }
//
//         //去除其他门户相同name或path的菜单
//         List<Zmenu> backList = new List<Zmenu>();
//         foreach (var zmenu in list)
//         {
//             if (porid == zmenu.porid)
//             {
//                 backList.Add(zmenu);
//             }
//         }
//
//         foreach (var zmenu in list)
//         {
//             if (porid != zmenu.porid)
//             {
//                 bool flag = false;
//                 foreach (var backMenu in backList)
//                 {
//                     if (backMenu.name == zmenu.name || backMenu.path == zmenu.path)
//                     {
//                         flag = true;
//                         break;
//                     }
//                 }
//
//                 if (!flag)
//                 {
//                     backList.Add(zmenu);
//                 }
//             }
//         }
//
//         return backList;
//     }
//
//     //使用递归方法建树
//     private List<Zmenu> BuildByRecursive(List<Zmenu> nodes)
//     {
//         List<Zmenu> list = new List<Zmenu>();
//         foreach (var node in nodes)
//         {
//             if (node.pid == null)
//             {
//                 list.Add(FindChildrenByTier(node, nodes));
//             }
//             else
//             {
//                 bool flag = false;
//                 foreach (var node2 in nodes)
//                 {
//                     if (node.pid == (node2.id))
//                     {
//                         flag = true;
//                         break;
//                     }
//                 }
//
//                 if (!flag)
//                 {
//                     list.Add(FindChildrenByTier(node, nodes));
//                 }
//             }
//         }
//
//         return list;
//     }
//
//     //递归查找子节点
//     private Zmenu FindChildrenByTier(Zmenu node, List<Zmenu> nodes)
//     {
//         foreach (var item in nodes)
//         {
//             if (node.id == item.pid)
//             {
//                 if (node.children == null)
//                 {
//                     node.children = new List<Zmenu>();
//                 }
//
//                 node.children.Add(FindChildrenByTier(item, nodes));
//             }
//         }
//
//         return node;
//     }
//
//     //如果用户没有菜单授权，则返回一个未授权的菜单
//     public List<Zmenu> FindNoPowerMenuList()
//     {
//         List<Zmenu> list = new List<Zmenu>();
//         Zmenu zmenu = new Zmenu();
//         zmenu.type = "M";
//         zmenu.id = "Home";
//         zmenu.name = "Home";
//         zmenu.path = "/home";
//         zmenu.component = "/home/noPower";
//         Zmeta zmeta = new Zmeta();
//         zmeta.title = "未授权";
//         zmeta.orderNo = 0;
//         zmeta.isHide = false;
//         zmeta.icon = "ele-Lock";
//         zmenu.meta = zmeta;
//         list.Add(zmenu);
//         return list;
//     }
//
//     //--------------------------------------API接口相关逻辑-------------------------------------------
//     //获取前台按钮集合，设置用户API权限集
//     private List<String> getBtnList(Zuser zuser)
//     {
//         List<String> btnList = new List<string>();
//         string sql =
//             @"select distinct m.id url,m.pos,m.code from sys_api_main m inner join sys_api_role_api rm on rm.mid=m.id 
//     inner join sys_api_role_org ru on ru.rid=rm.rid where  m.avtag="+Db.True+" and ru.oid in (" + zuser.conds + ") and code<>0";
//         List<Yapi> apiList = _repo.Context.Ado.SqlQuery<Yapi>(sql);
//         int posSum = SysPermApiCache.AUTHPOS + 1; //取出最大权限位
//         long[] permArr = new long[posSum];
//         foreach (var api in apiList)
//         {
//             for (int i = 0; i < posSum; i++)
//             {
//                 if (i == api.pos)
//                 {
//                     permArr[i] += api.code;
//                 }
//             }
//
//             btnList.Add(api.url);
//         }
//
//         string perms = "";
//         for (int i = 0; i < permArr.Length; i++)
//         {
//             perms += permArr[i] + ";";
//         }
//
//         zuser.perms = perms != "" ? perms.Substring(0, perms.Length - 1) : "0";
//         return btnList;
//     }
//
//     //缓存用户门户、菜单、按钮到缓存表
//     private void updateOrgUserCache(Zuser zuser, List<Zmenu> menuList, List<string> btnList, List<Zportal> portalList)
//     {
//         string menus = _json.Serialize(menuList);
//         string portals = _json.Serialize(portalList);
//         StringBuilder btns = new StringBuilder();
//         foreach (var str in btnList)
//         {
//             btns.Append(str).Append(";");
//         }
//
//         SysOrgUserCache cache = new SysOrgUserCache();
//         cache.id = zuser.id;
//         cache.conds = zuser.conds;
//         cache.menus = menus;
//         cache.btns = btns.ToString();
//         cache.perms = zuser.perms;
//         cache.portals = portals;
//
//         var isExists = _repo.Context.Queryable<SysOrgUserCache>().Any(it => it.id == zuser.id);
//         if (isExists)
//         {
//             _repo.Context.Updateable(cache).ExecuteCommand();
//         }
//         else
//         {
//             _repo.Context.Insertable(cache).ExecuteCommand();
//         }
//
//         _repo.Context.Updateable(new SysOrgUser() { id = zuser.id, catag = true })
//             .UpdateColumns(it => new { it.catag }).ExecuteCommand();
//     }
//
//
//     //缓存Coop用户门户、菜单、按钮到缓存表
//     private void updateCoopUserCache(Zuser zuser, List<Zmenu> menuList, List<string> btnList, List<Zportal> portalList)
//     {
//         string menus = _json.Serialize(menuList);
//         string portals = _json.Serialize(portalList);
//         StringBuilder btns = new StringBuilder();
//         foreach (var str in btnList)
//         {
//             btns.Append(str).Append(";");
//         }
//
//         SysCoopUserCache cache = new SysCoopUserCache();
//         cache.id = zuser.id;
//         cache.conds = zuser.conds;
//         cache.menus = menus;
//         cache.btns = btns.ToString();
//         cache.perms = zuser.perms;
//         cache.portals = portals;
//
//         var isExists = _repo.Context.Queryable<SysCoopUserCache>().Any(it => it.id == zuser.id);
//         if (isExists)
//         {
//             _repo.Context.Updateable(cache).ExecuteCommand();
//         }
//         else
//         {
//             _repo.Context.Insertable(cache).ExecuteCommand();
//         }
//
//         _repo.Context.Updateable(new SysCoopUser() { id = zuser.id, catag = true })
//             .UpdateColumns(it => new { it.catag }).ExecuteCommand();
//     }
//
//     private void listAdd(List<Zmenu> list, List<Zmenu> shoudAddList)
//     {
//         List<Zmenu> list2 = new List<Zmenu>();
//         foreach (var zmenu2 in shoudAddList)
//         {
//             bool flag = false;
//             foreach (var zmenu in list)
//             {
//                 if (zmenu2.id == zmenu.id)
//                 {
//                     flag = true;
//                     break;
//                 }
//             }
//
//             if (!flag)
//             {
//                 list2.Add(zmenu2);
//             }
//         }
//         list.AddRange(list2);
//     }
//
// }