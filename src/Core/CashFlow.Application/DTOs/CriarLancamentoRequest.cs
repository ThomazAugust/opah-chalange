using CashFlow.Domain.Enums;

namespace CashFlow.Application.DTOs;

public sealed record CriarLancamentoRequest(
    string Descricao,
    decimal Valor,
    TipoLancamento Tipo,
    DateTimeOffset DataLancamento,
    Guid UsuarioId);
