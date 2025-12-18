using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Vben.Common.Core;
using Vben.Common.Sqlsugar.Mvc.Entity;

namespace Vben.Common.Sqlsugar.Config;

public static class DbExtension
{
    public static IApplicationBuilder DbFirst(this IApplicationBuilder app)
    {
        SqlSugarScope _sqlSugarScope = (SqlSugarScope)app.ApplicationServices.GetRequiredService<ISqlSugarClient>();
        var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        var vbenProvider = _sqlSugarScope.GetConnectionScope("primary");
        vbenProvider.CodeFirst.InitTables(typeof(SysOrg));
        List<string> cls=MyApp.GetOptions<List<string>>("DbFirstScanModules");
        foreach (var cl in cls)
        {
            var assemblies = Directory.GetFiles(path, cl+".dll").Select(Assembly.LoadFrom)
                .ToArray();

            var coreModelTypes = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x.IsClass && x.Namespace != null && x.Namespace.StartsWith("Vben")).ToList();
            coreModelTypes.ForEach(t =>
            {
                var customAttributeDatas = t.CustomAttributes;
                foreach (var attribute in customAttributeDatas)
                {
                    if (attribute.ToString().Contains("SugarTable"))
                    {
                        var tAtt = t.GetCustomAttribute<TenantAttribute>();
                        if (tAtt == null)
                        {
                            vbenProvider.CodeFirst.InitTables(t);
                        }
                        else
                        {
                            _sqlSugarScope.GetConnectionScope(tAtt.configId).CodeFirst.InitTables(t);
                        }
                    }
                }
            });
        }
        return app;
    }
}