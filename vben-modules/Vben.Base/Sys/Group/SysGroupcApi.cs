namespace Vben.Base.Sys.Group;

[Route("sys/groupc")]
[ApiDescriptionSettings("Sys", Tag = "组织架构-群组分类")]
public class SysGroupcApi(SysGroupCateService service) : ControllerBase
{
    /// <summary>
    /// 获取分类treeTable数据
    /// </summary>
    [HttpGet("tree")]
    public async Task<dynamic> GetTree(string id)
    {
        var treeList = await service.repo.Context
            .Queryable<SysGroupCate>()
            .WhereIF(!string.IsNullOrWhiteSpace(id), t => t.id != id)
            .OrderBy(it => it.ornum)
            .ToTreeAsync(it => it.children, it => it.pid, null);
        return treeList;
    }

    /// <summary>
    /// 获取单个分类详细信息
    /// </summary>
    [HttpGet("info/{id}")]
    public async Task<SysGroupCate> GetInfo(string id)
    {
        var cate = await service.SingleAsync(id);
        // if (cate.pid != null)
        // {
        //     cate.pname = await _service.repo.Context.Queryable<SysGroupCate>()
        //         .Where(it => it.id == cate.pid).Select(it => it.name).SingleAsync();
        // }
        return cate;
    }

    /// <summary>
    /// 新增分类
    /// </summary>
    [HttpPost]
    public async Task<string> Post([FromBody] SysGroupCate cate)
    {
        int count = await service.repo.Context.Queryable<SysGroupCate>().Where(t => t.pid == cate.pid).CountAsync();
        cate.ornum = ++count;
        return await service.InsertAsync(cate);
    }

    /// <summary>
    /// 更新分类
    /// </summary>
    [HttpPut]
    public async Task<string> Put([FromBody] SysGroupCate cate)
    {
        return await service.UpdateAsync(cate);
    }

    /// <summary>
    /// 删除分类
    /// </summary>
    [HttpDelete("{ids}")]
    public async Task Delete(string ids)
    {
        var idArr = ids.Split(",");
        foreach (var id in idArr)
        {
            var cateCount = await
                service.repo.Context.Queryable<SysGroupCate>().Where(it => it.pid == id).CountAsync();
            if (cateCount > 0)
            {
                throw new Exception("此分类下有子分类，无法删除");
            }

            var mainCount = await
                service.repo.Context.Queryable<SysGroup>().Where(it => it.catid == id).CountAsync();

            if (mainCount > 0)
            {
                throw new Exception("此分类下有群组，无法删除");
            }
        }

        await service.DeleteAsync(ids);
    }

    /// <summary>
    /// 移动分类
    /// </summary>
    [HttpPost("move")]
    public async Task PostMove([FromBody] TreeMovePo po)
    {
        await service.Move(po);
    }
}