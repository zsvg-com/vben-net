using Vben.Common.Core;
using Vben.Common.Sqlsugar;

var builder = WebApplication.CreateBuilder(args);
MyApp.Configuration = builder.Configuration;

builder.Configuration.AddJsonFile("Properties/Configs/web.json", optional: true, reloadOnChange: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("Properties/Configs/web.Development.json", optional: true, reloadOnChange: true);
}
else if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("Properties/Configs/web.Production.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddJsonFile("Properties/Configs/Cache.json");

builder.Inject();
builder.Host.UseSerilogDefault();
builder.Services.AddDb();

var app = builder.Build();


MyApp.ServiceProvider = app.Services;
MyApp.WebHostEnvironment = app.Environment;
app.DbFirst();

app.Run();