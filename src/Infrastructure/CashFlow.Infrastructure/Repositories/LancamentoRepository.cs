using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Interfaces;
using Dapper;

namespace CashFlow.Infrastructure.Repositories;

public class LancamentoRepository(ConnectionFactory connectionFactory) : ILancamentoRepository
{
    public async Task<Guid> AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO lancamentos (id, descricao, valor, tipo, data_lancamento, usuario_id)
            VALUES (@Id, @Descricao, @Valor, @Tipo, @DataLancamento, @UsuarioId);
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            lancamento.Id,
            lancamento.Descricao,
            lancamento.Valor,
            Tipo = (int)lancamento.Tipo,
            lancamento.DataLancamento,
            lancamento.UsuarioId
        }, cancellationToken: cancellationToken));

        return lancamento.Id;
    }

    public async Task<IReadOnlyCollection<Lancamento>> ListByDateAsync(DateOnly data, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT id, descricao, valor, tipo, data_lancamento, usuario_id
            FROM lancamentos
            WHERE DATE(data_lancamento) = @Data;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var rows = await connection.QueryAsync<RawLancamento>(new CommandDefinition(sql, new { Data = data.ToDateTime(TimeOnly.MinValue) }, cancellationToken: cancellationToken));

        return rows.Select(row => new Lancamento(
            row.Id,
            row.Descricao,
            row.Valor,
            (ModalidadeLancamento)row.Tipo,
            new DateTimeOffset(DateTime.SpecifyKind(row.DataLancamento, DateTimeKind.Utc)),
            row.UsuarioId)).ToArray();
    }

    private sealed record RawLancamento(Guid Id, string Descricao, decimal Valor, int Tipo, DateTime DataLancamento, Guid UsuarioId);
}
