using Consolidation.Worker;
using CashFlow.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{
	var builder = Host.CreateApplicationBuilder(args);
	builder.Services.AddSerilog((services, configuration) => configuration
		.ReadFrom.Configuration(builder.Configuration)
		.ReadFrom.Services(services));
	builder.Services.AddCashFlowInfrastructure(builder.Configuration);
	builder.Services.AddHostedService<Worker>();

	var host = builder.Build();
	host.Run();
}
catch (Exception exception)
{
	Log.Fatal(exception, "A aplicação encerrou inesperadamente.");
}
finally
{
	Log.CloseAndFlush();
}
