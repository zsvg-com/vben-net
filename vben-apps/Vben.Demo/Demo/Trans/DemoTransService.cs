using Vben.Admin.Demo.Single.Main;

namespace Vben.Admin.Demo.Trans;

[Service]
public class DemoTransService(SqlSugarRepository<DemoSingleMain> Repo)
{
    public async Task Insert()
    {
        
        DemoSingleMain main1=new DemoSingleMain();
        main1.id =  YitIdHelper.NextId(); 
        main1.cruid =  "111"; 
        await Repo.InsertAsync(main1);
        
        
        DemoSingleMain main2=new DemoSingleMain();
        main2.id =  YitIdHelper.NextId(); 
        main2.cruid =  "fdsafdasfdsafdsdasfdsafdfdsafdasfdsafdsdasfdsafdasfdsafdasfdsafdasfdsafdasfdsafafsdaaasfdsafdasfdsafdasfdsafdasfdsafafsdaa"; 
        await Repo.InsertAsync(main2);
        
        
    }

}