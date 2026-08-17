using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interfaces;

public interface ISaldoConsolidadoRepository
{
    Task<SaldoConsolidado> GetByDateAsync(DateOnly data, CancellationToken cancellationToken = default);
    Task UpsertAsync(SaldoConsolidado saldoConsolidado, CancellationToken cancellationToken = default);
}
