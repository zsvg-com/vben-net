namespace Vben.Base.Auth.Captcha;

[Route("")]
[ApiDescriptionSettings("Auth", Tag = "验证码")]
public class AuthCapchaApi : ControllerBase
{
    [HttpGet("/auth/code")]
    [AllowAnonymous]
    public CaptchaVo GetCode()
    {
        CaptchaVo captchaVo = new CaptchaVo();
        captchaVo.captchaEnabled = false;
        return captchaVo;
    }
}