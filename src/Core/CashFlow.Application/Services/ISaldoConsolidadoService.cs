using CashFlow.Application.DTOs;

namespace CashFlow.Application.Services;

public interface ISaldoConsolidadoService
{
    Task<SaldoDiarioResponse?> ObterPorDataAsync(DateOnly data, CancellationToken cancellationToken = default);
    Task ReprocessarDiaAsync(DateOnly data, CancellationToken cancellationToken = default);
}
