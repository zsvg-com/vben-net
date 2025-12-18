using Vben.Base.Sys.Org.User;

namespace Vben.Base.Sys.Profile;

[Route("system/user/profile")]
[ApiDescriptionSettings("Sys", Tag = "个人信息")]
public class SysProfileApi(SysOrgUserService userService) : ControllerBase
{

    [HttpGet]
    public async Task<dynamic> GetProfile()
    {
        SysOrgUser user= await userService.SingleAsync(XuserUtil.getUserId());
        ProfileUserVo vo=new ProfileUserVo();
        vo.userId=user.id;
        vo.email=user.email;
        vo.sex=user.sex;
        vo.deptId=user.depid;
        vo.userName = user.usnam; 
        vo.userType=user.type+"";
        vo.nickName = user.name;
        vo.phonenumber= user.monum;
        // vo.deptName = user.d setDeptName(user.getDepna());
        vo.loginIp=user.loip;
        vo.loginDate=user.lotim;
        vo.avatar=user.avatar;
        var todo = new Dictionary<string, object>
        {
            {"user", vo}
        };
        return todo;
    }
}