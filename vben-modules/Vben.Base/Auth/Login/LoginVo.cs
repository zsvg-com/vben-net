namespace Vben.Base.Auth.Login;


public class LoginVo
{
    
    public string access_token { get; set; }
    
    public string refresh_token { get; set; }
    
    public long expire_in { get; set; }

    public long? refresh_expire_in { get; set; }
    
    public string client_id { get; set; }
    
    public string scope { get; set; }
    
    public string openid { get; set; }
    
}