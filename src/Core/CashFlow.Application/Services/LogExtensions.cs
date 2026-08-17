using Microsoft.Extensions.Logging;

namespace CashFlow.Application.Services;

public static partial class LogExtensions
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Processando lançamento ID: {LancamentoId} para o usuário: {UsuarioId}")]
    public static partial void LogProcessandoLancamento(this ILogger logger, Guid lancamentoId, Guid usuarioId);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = "Lançamento ID: {LancamentoId} registrado com sucesso")]
    public static partial void LogLancamentoRegistrado(this ILogger logger, Guid lancamentoId);

    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Error,
        Message = "Falha ao processar lançamento ID: {LancamentoId}. Erro: {MensagemErro}")]
    public static partial void LogErroProcessamento(this ILogger logger, Exception exception, Guid lancamentoId, string mensagemErro);
}