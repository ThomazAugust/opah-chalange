using CashFlow.Application.DTOs;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;

namespace CashFlow.Application.Services;

public class LancamentoService(
    ILancamentoRepository lancamentoRepository,
    ILancamentoEventQueue lancamentoEventQueue) : ILancamentoService
{
    public async Task<LancamentoResponse> RegistrarAsync(CriarLancamentoRequest request, CancellationToken cancellationToken = default)
    {
        var lancamento = new Lancamento(
            Guid.NewGuid(),
            request.Descricao,
            request.Valor,
            request.Tipo,
            request.DataLancamento,
            request.UsuarioId);

        var id = await lancamentoRepository.AddAsync(lancamento, cancellationToken);

        // A fila é desacoplada para garantir resiliência caso a consolidação esteja indisponível.
        await lancamentoEventQueue.EnqueueAsync(lancamento, cancellationToken);

        return new LancamentoResponse(
            id,
            lancamento.Descricao,
            lancamento.Valor,
            lancamento.Tipo,
            lancamento.DataLancamento,
            lancamento.UsuarioId);
    }
}
