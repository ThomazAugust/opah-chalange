using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class SaldoConsolidado
{
    public DateOnly Data { get; private set; }
    public decimal TotalCreditos { get; private set; }
    public decimal TotalDebitos { get; private set; }
    public decimal SaldoFinal { get; private set; }
    public DateTimeOffset UltimaAtualizacao { get; private set; }

    public SaldoConsolidado(DateOnly data)
    {
        Data = data;
        UltimaAtualizacao = DateTimeOffset.UtcNow;
    }

    public SaldoConsolidado(DateOnly data, decimal totalCreditos, decimal totalDebitos, DateTimeOffset ultimaAtualizacao)
    {
        if (totalCreditos < 0)
        {
            throw new ArgumentException("O total de créditos não pode ser negativo.", nameof(totalCreditos));
        }

        if (totalDebitos < 0)
        {
            throw new ArgumentException("O total de débitos não pode ser negativo.", nameof(totalDebitos));
        }

        Data = data;
        TotalCreditos = totalCreditos;
        TotalDebitos = totalDebitos;
        SaldoFinal = TotalCreditos - TotalDebitos;
        UltimaAtualizacao = ultimaAtualizacao;
    }

    public void AplicarLancamento(decimal valor, ModalidadeLancamento tipo)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do lançamento deve ser maior que zero.", nameof(valor));
        }

        if (tipo == ModalidadeLancamento.Credito)
        {
            TotalCreditos += valor;
        }
        else
        {
            TotalDebitos += valor;
        }

        SaldoFinal = TotalCreditos - TotalDebitos;
        UltimaAtualizacao = DateTimeOffset.UtcNow;
    }
}
