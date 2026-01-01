namespace Vben.Admin.Demo.Trans;

[Route("demo/transaction")]
[ApiDescriptionSettings(Tag = "事务测试")]
public class DemoTransController(DemoTransService service) : ControllerBase
{
    /// <summary>
    /// 事务测试
    /// </summary>
    [HttpPost]
    [Transactional]
    public async Task Post()
    {
        await service.Insert();
    }
}