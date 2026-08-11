using CashFlow.Domain.Enums;

namespace CashFlow.Application.DTOs;

public sealed record LancamentoResponse(
    Guid Id,
    string Descricao,
    decimal Valor,
    TipoLancamento Tipo,
    DateTimeOffset DataLancamento,
    Guid UsuarioId);
