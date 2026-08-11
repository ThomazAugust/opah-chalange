using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class Lancamento
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public TipoLancamento Tipo { get; private set; }
    public DateTimeOffset DataLancamento { get; private set; }
    public Guid UsuarioId { get; private set; }

    public Lancamento(Guid id, string descricao, decimal valor, TipoLancamento tipo, DateTimeOffset dataLancamento, Guid usuarioId)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do lançamento deve ser maior que zero.", nameof(valor));
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição do lançamento é obrigatória.", nameof(descricao));
        }

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Descricao = descricao.Trim();
        Valor = valor;
        Tipo = tipo;
        DataLancamento = dataLancamento;
        UsuarioId = usuarioId;
    }
}
