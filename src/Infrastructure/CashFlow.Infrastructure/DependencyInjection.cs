using CashFlow.Application.Services;
using CashFlow.Domain.Interfaces;
using CashFlow.Infrastructure.Messaging;
using CashFlow.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCashFlowInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILancamentoRepository, LancamentoRepository>();
        services.AddScoped<ISaldoConsolidadoRepository, SaldoConsolidadoRepository>();
        services.AddScoped<ILancamentoEventQueue, PostgresLancamentoEventQueue>();

        services.AddScoped<ILancamentoService, LancamentoService>();
        services.AddScoped<ISaldoConsolidadoService, SaldoConsolidadoService>();

        services.AddSingleton<ConnectionFactory>(_ =>
            new ConnectionFactory(configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.")));

        return services;
    }
}
