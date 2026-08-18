using CashFlow.Application.DTOs;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CashFlow.Application.Services;

public class LancamentoService(
    ILancamentoRepository lancamentoRepository,
    ILogger<LancamentoService> logger) : ILancamentoService
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

        logger.LogProcessandoLancamento(lancamento.Id, lancamento.UsuarioId);

        try
        {
            var id = await lancamentoRepository.AddAsync(lancamento, cancellationToken);

            logger.LogLancamentoRegistrado(id);

            return new LancamentoResponse(
                id,
                lancamento.Descricao,
                lancamento.Valor,
                lancamento.Tipo,
                lancamento.DataLancamento,
                lancamento.UsuarioId);
        }
        catch (Exception exception)
        {
            logger.LogErroProcessamento(exception, lancamento.Id, exception.Message);
            throw;
        }
    }

    public async Task<IReadOnlyCollection<LancamentoResponse>> BuscarAsync(Guid? id, Guid? usuarioId, ModalidadeLancamento? tipo, CancellationToken cancellationToken = default)
    {
        var lancamentos = await lancamentoRepository.BuscarAsync(id, usuarioId, tipo, cancellationToken);

        return lancamentos.Select(lancamento => new LancamentoResponse(
            lancamento.Id,
            lancamento.Descricao,
            lancamento.Valor,
            lancamento.Tipo,
            lancamento.DataLancamento,
            lancamento.UsuarioId)).ToArray();
    }
}
