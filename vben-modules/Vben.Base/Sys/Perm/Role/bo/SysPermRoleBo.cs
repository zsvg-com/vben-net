namespace Vben.Base.Sys.Perm.Role.bo;

public class SysPermRoleBo
{
    public long id { get; set; }

    public string name { get; set; }
    
    public string notes { get; set; }
    
    public bool avtag { get; set; } = true;
    
    public int ornum { get; set; }
    
    public DateTime? crtim { get; set; } = DateTime.Now;

    public DateTime? uptim { get; set; }

    public SysOrg crman { get; set; }

    public string cruid { get; set; }

    public SysOrg upman { get; set; }

    public string upuid { get; set; }

    public List<SysOrg> orgs { get; set; }
    
    public List<long> menus { get; set; }
    
    public List<long> apis { get; set; }

}