using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Interfaces;

public interface ILancamentoRepository
{
    Task<Guid> AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Lancamento>> ListByDateAsync(DateOnly data, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Lancamento>> BuscarAsync(Guid? id, Guid? usuarioId, ModalidadeLancamento? tipo, CancellationToken cancellationToken = default);
}
