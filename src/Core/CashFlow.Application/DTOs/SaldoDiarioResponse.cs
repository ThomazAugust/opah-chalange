namespace CashFlow.Application.DTOs;

public sealed record SaldoDiarioResponse(
    DateOnly Data,
    decimal TotalCreditos,
    decimal TotalDebitos,
    decimal SaldoFinal,
    DateTimeOffset UltimaAtualizacao);
