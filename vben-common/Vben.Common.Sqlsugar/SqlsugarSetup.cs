using Vben.Common.Core;
using Vben.Common.Sqlsugar.Config;

namespace Vben.Common.Sqlsugar;

public static class SqlSugarSetup
{
    public static void AddDb(this IServiceCollection services)
    {
        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            DbConfig dbConfig = MyApp.GetOptions<DbConfig>();
            if (dbConfig.ConnectionConfigs[0].DbType == DbType.PostgreSQL)
            {
               Db.True = "true";
            }
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityService = (type, column) =>
                {
                    if (column.DataType == "varchar(max)")
                    {
                        if (dbConfig.ConnectionConfigs[0].DbType == DbType.MySql)
                        {
                            column.DataType = "longtext";
                        }
                        else if (dbConfig.ConnectionConfigs[0].DbType == DbType.Oracle)
                        {
                            column.DataType = "clob";
                        }
                        else if (dbConfig.ConnectionConfigs[0].DbType == DbType.PostgreSQL)
                        {
                            column.DataType = "text";
                        }
                    }
                    else  if (column.DataType == "bool")
                    {
                        if (dbConfig.ConnectionConfigs[0].DbType == DbType.MySql)
                        {
                            column.DataType = "bit";
                        }
                        else if (dbConfig.ConnectionConfigs[0].DbType == DbType.Oracle)
                        {
                            column.DataType = "number(1)";
                        }
                        else if (dbConfig.ConnectionConfigs[0].DbType == DbType.PostgreSQL)
                        {
                            column.DataType = "int2";
                        }
                    }
                },
                EntityNameService = (type, entity) => { entity.IsDisabledDelete = true; },
            };
            dbConfig.ConnectionConfigs.ForEach(config =>
            {
                config.ConfigureExternalServices = configureExternalServices;
            });

            SqlSugarScope sqlSugar = new(dbConfig.ConnectionConfigs, db =>
            {
                dbConfig.ConnectionConfigs.ForEach(config =>
                {
                    var dbProvider = db.GetConnectionScope((string)config.ConfigId);

                    // 设置超时时间
                    dbProvider.Ado.CommandTimeOut = 120;

                    // 打印SQL语句
                    if (dbConfig.ShowSql)
                    {
                        dbProvider.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                                Console.ForegroundColor = ConsoleColor.Green;
                            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) ||
                                sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                                Console.ForegroundColor = ConsoleColor.White;
                            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                                Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" +
                                              UtilMethods.GetSqlString(DbType.MySql, sql, pars) + "\r\n");
                        };
                    }

                    dbProvider.Aop.OnError = (ex) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        var pars = db.Utilities.SerializeObject(
                            ((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                        Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" +
                                          UtilMethods.GetSqlString(DbType.MySql, ex.Sql,
                                              (SugarParameter[])ex.Parametres) + "\r\n");
                    };
                });
            });

            return sqlSugar;
        });
        services.AddScoped(typeof(SqlSugarRepository<>)); // 注册仓储
    }
}