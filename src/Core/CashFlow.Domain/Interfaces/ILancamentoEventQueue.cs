using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interfaces;

public interface ILancamentoEventQueue
{
    Task EnqueueAsync(Lancamento lancamento, CancellationToken cancellationToken = default);
}
