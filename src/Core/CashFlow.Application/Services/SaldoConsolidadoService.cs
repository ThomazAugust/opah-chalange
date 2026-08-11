using CashFlow.Application.DTOs;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;

namespace CashFlow.Application.Services;

public class SaldoConsolidadoService(
    ISaldoConsolidadoRepository saldoConsolidadoRepository,
    ILancamentoRepository lancamentoRepository) : ISaldoConsolidadoService
{
    public async Task<SaldoDiarioResponse?> ObterPorDataAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        var saldo = await saldoConsolidadoRepository.GetByDateAsync(data, cancellationToken);
        if (saldo is null)
        {
            return null;
        }

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
            saldo.AplicarLancamento(lancamento.Valor, lancamento.Tipo);
        }

        await saldoConsolidadoRepository.UpsertAsync(saldo, cancellationToken);
    }
}
