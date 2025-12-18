using Vben.Base.Auth.Login;

namespace Vben.Base.Auth;

public class PasswordLoginBo:LoginBo
{
    public string username { get; set; }
    
    public string password { get; set; }
    
}