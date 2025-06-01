using m42Service;
using m42Service.DSL;
using m42Service.Helpers;
using M42Service.DSL;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<TimersDSL>();
builder.Services.AddSingleton<M42Dsl>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<ConfigManager>();
var host = builder.Build();
host.Run();
