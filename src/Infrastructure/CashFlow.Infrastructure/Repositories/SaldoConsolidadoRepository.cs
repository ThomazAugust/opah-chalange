using CashFlow.Domain.Entities;
using CashFlow.Domain.Interfaces;
using Dapper;

namespace CashFlow.Infrastructure.Repositories;

public class SaldoConsolidadoRepository(ConnectionFactory connectionFactory) : ISaldoConsolidadoRepository
{
    public async Task<SaldoConsolidado> GetByDateAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                COALESCE(SUM(valor) FILTER (WHERE tipo = 1), 0) AS total_creditos,
                COALESCE(SUM(valor) FILTER (WHERE tipo = 2), 0) AS total_debitos
            FROM lancamentos
            WHERE DATE(data_lancamento) = @Data;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var queryParameters = new { Data = data.ToDateTime(TimeOnly.MinValue) };
        var totais = await connection.QuerySingleAsync<RawTotais>(new CommandDefinition(sql, queryParameters, cancellationToken: cancellationToken));

        return new SaldoConsolidado(data, totais.TotalCreditos, totais.TotalDebitos, DateTimeOffset.UtcNow);
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

    private sealed record RawTotais(decimal TotalCreditos, decimal TotalDebitos);
}
