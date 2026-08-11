using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;
using Dapper;

namespace CashFlow.Infrastructure.Messaging;

public class PostgresLancamentoEventQueue(ConnectionFactory connectionFactory) : ILancamentoEventQueue
{
    public async Task EnqueueAsync(Lancamento lancamento, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO lancamentos_queue (lancamento_id, payload, criado_em)
            VALUES (@LancamentoId, @Payload, @CriadoEm);
            """;

        var payload = System.Text.Json.JsonSerializer.Serialize(new
        {
            lancamento.Id,
            lancamento.Descricao,
            lancamento.Valor,
            Tipo = (int)lancamento.Tipo,
            lancamento.DataLancamento,
            lancamento.UsuarioId
        });

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            LancamentoId = lancamento.Id,
            Payload = payload,
            CriadoEm = DateTimeOffset.UtcNow
        }, cancellationToken: cancellationToken));
    }
}
