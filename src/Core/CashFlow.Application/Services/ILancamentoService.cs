using CashFlow.Application.DTOs;
using CashFlow.Domain.Enums;

namespace CashFlow.Application.Services;

public interface ILancamentoService
{
    Task<LancamentoResponse> RegistrarAsync(CriarLancamentoRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LancamentoResponse>> BuscarAsync(Guid? id, Guid? usuarioId, ModalidadeLancamento? tipo, CancellationToken cancellationToken = default);
}
