using Consolidation.Worker;
using CashFlow.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddCashFlowInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
