using CashFlow.Application.DTOs;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CashFlow.Application.Services;

public class SaldoConsolidadoService(
    ISaldoConsolidadoRepository saldoConsolidadoRepository,
    ILancamentoRepository lancamentoRepository,
    ILogger<SaldoConsolidadoService> logger) : ISaldoConsolidadoService
{
    public async Task<SaldoDiarioResponse?> ObterPorDataAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        var saldo = await saldoConsolidadoRepository.GetByDateAsync(data, cancellationToken);

        return new SaldoDiarioResponse(
            saldo.Data,
            saldo.TotalCreditos,
            saldo.TotalDebitos,
            saldo.SaldoFinal,
            saldo.UltimaAtualizacao);
    }

    public async Task ReprocessarDiaAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        var lancamentos = await lancamentoRepository.ListByDateAsync(data, cancellationToken);
        var saldo = new SaldoConsolidado(data);

        foreach (var lancamento in lancamentos)
        {
            logger.LogProcessandoLancamento(lancamento.Id, lancamento.UsuarioId);
            saldo.AplicarLancamento(lancamento.Valor, lancamento.Tipo);
        }

        await saldoConsolidadoRepository.UpsertAsync(saldo, cancellationToken);
    }
}
