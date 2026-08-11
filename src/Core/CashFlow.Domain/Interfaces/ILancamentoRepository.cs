using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interfaces;

public interface ILancamentoRepository
{
    Task<Guid> AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Lancamento>> ListByDateAsync(DateOnly data, CancellationToken cancellationToken = default);
}
