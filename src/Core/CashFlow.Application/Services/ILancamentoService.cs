using CashFlow.Application.DTOs;

namespace CashFlow.Application.Services;

public interface ILancamentoService
{
    Task<LancamentoResponse> RegistrarAsync(CriarLancamentoRequest request, CancellationToken cancellationToken = default);
}
