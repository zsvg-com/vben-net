namespace Vben.Common.Core.Manager;

public interface IUserManager
{
    string Account { get; }
    string Name { get; }
    bool SuperAdmin { get; }
    // SysOrgUser User { get; }
    string UserId { get; }
    string Perms { get; }

    string DeptId { get; }

    string Label { get; }

    string Type { get; }

    string Conds { get; }

    // Task<SysOrgUser> CheckUserAsync(string userId, bool tracking = true);
    // Task<SysOrgUser> GetUserEmpInfo(string userId);
}