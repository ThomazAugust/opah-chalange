using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;
using Dapper;

namespace CashFlow.Infrastructure.Repositories;

public class SaldoConsolidadoRepository(ConnectionFactory connectionFactory) : ISaldoConsolidadoRepository
{
    public async Task<SaldoConsolidado?> GetByDateAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT data, total_creditos, total_debitos, saldo_final, ultima_atualizacao
            FROM saldos_consolidados
            WHERE data = @Data;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var row = await connection.QueryFirstOrDefaultAsync<RawSaldo>(new CommandDefinition(sql, new { Data = data }, cancellationToken: cancellationToken));
        if (row is null)
        {
            return null;
        }

        return new SaldoConsolidado(row.Data, row.TotalCreditos, row.TotalDebitos, row.UltimaAtualizacao);
    }

    public async Task UpsertAsync(SaldoConsolidado saldoConsolidado, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO saldos_consolidados (data, total_creditos, total_debitos, saldo_final, ultima_atualizacao)
            VALUES (@Data, @TotalCreditos, @TotalDebitos, @SaldoFinal, @UltimaAtualizacao)
            ON CONFLICT (data)
            DO UPDATE SET
              total_creditos = EXCLUDED.total_creditos,
              total_debitos = EXCLUDED.total_debitos,
              saldo_final = EXCLUDED.saldo_final,
              ultima_atualizacao = EXCLUDED.ultima_atualizacao;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            saldoConsolidado.Data,
            saldoConsolidado.TotalCreditos,
            saldoConsolidado.TotalDebitos,
            saldoConsolidado.SaldoFinal,
            saldoConsolidado.UltimaAtualizacao
        }, cancellationToken: cancellationToken));
    }

    private sealed record RawSaldo(DateOnly Data, decimal TotalCreditos, decimal TotalDebitos, decimal SaldoFinal, DateTimeOffset UltimaAtualizacao);
}
