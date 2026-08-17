using CashFlow.Application.Services;
using CashFlow.Domain.Interfaces;
using CashFlow.Infrastructure.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCashFlowInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Necessário para o Dapper materializar registros a partir de colunas snake_case (ex.: total_creditos -> TotalCreditos).
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<ILancamentoRepository, LancamentoRepository>();
        services.AddScoped<ISaldoConsolidadoRepository, SaldoConsolidadoRepository>();

        services.AddScoped<ILancamentoService, LancamentoService>();
        services.AddScoped<ISaldoConsolidadoService, SaldoConsolidadoService>();

        services.AddSingleton<ConnectionFactory>(_ =>
            new ConnectionFactory(configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.")));

        return services;
    }
}
