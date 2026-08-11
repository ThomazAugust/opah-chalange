using CashFlow.Application.Services;

namespace Consolidation.Worker;

public class Worker(
    ILogger<Worker> logger,
    ISaldoConsolidadoService saldoConsolidadoService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var dataAtual = DateOnly.FromDateTime(DateTime.UtcNow);
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
