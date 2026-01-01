using Vben.Base.Sys.User;
using Vben.Base.Tool.Oss.Main;
using Vben.Common.Core.Token;

namespace Vben.Base.Sys.Profile;

[Route("system/user/profile")]
[ApiDescriptionSettings("Sys", Tag = "个人信息")]
public class SysProfileApi(SysUserService userService,ToolOssMainService ossMainService) : ControllerBase
{

    [HttpGet]
    public async Task<dynamic> GetProfile()
    {
        SysUser user= await userService.SingleAsync(LoginHelper.UserId);
        ProfileUserVo vo=new ProfileUserVo();
        vo.userId=user.id;
        vo.email=user.email;
        vo.gender=user.gender;
        vo.deptId=user.depid;
        vo.userName = user.username; 
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
    
    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    public async Task<Dictionary<string, object>> PostAvatar(IFormFile avatarfile)
    {
        var sysFile = await ossMainService.UploadFile(avatarfile);
        
        string url="/api/tool/oss/main/show?id="+sysFile.id;

        string userId = LoginHelper.UserId;
        userService.UpdateAvatar(LoginHelper.UserId, url);
        var avatarVo = new Dictionary<string, object>
        {
            {"imgUrl", url}
        };
        return avatarVo;
    }
    
    [HttpPost]
    public async Task Post([FromBody] PasswordBo bo)
    {
        //todo 修改密码
    }

    
    
    
    
}