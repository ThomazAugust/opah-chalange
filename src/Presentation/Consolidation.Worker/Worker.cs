using CashFlow.Application.Services;

namespace Consolidation.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var dataAtual = DateOnly.FromDateTime(DateTime.UtcNow);

                using var scope = scopeFactory.CreateScope();
                var saldoConsolidadoService = scope.ServiceProvider.GetRequiredService<ISaldoConsolidadoService>();
                await saldoConsolidadoService.ReprocessarDiaAsync(dataAtual, stoppingToken);
                logger.LogInformation("Consolidação executada para {Data} em {Horario}.", dataAtual, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao consolidar saldo diário.");
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
