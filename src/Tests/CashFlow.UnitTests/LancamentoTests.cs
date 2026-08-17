using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CashFlow.UnitTests;

public class LancamentoTests
{
    [Fact]
    public void Deve_Criar_Lancamento_Quando_Valor_For_Maior_Que_Zero()
    {
        var lancamento = new Lancamento(
            Guid.NewGuid(),
            "Pagamento de cliente",
            150.50m,
            ModalidadeLancamento.Credito,
            DateTimeOffset.UtcNow,
            Guid.NewGuid());

        Assert.Equal(150.50m, lancamento.Valor);
        Assert.Equal(ModalidadeLancamento.Credito, lancamento.Tipo);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_For_Menor_Ou_Igual_A_Zero()
    {
        var action = () => new Lancamento(
            Guid.NewGuid(),
            "Despesa",
            0m,
            ModalidadeLancamento.Debito,
            DateTimeOffset.UtcNow,
            Guid.NewGuid());

        Assert.Throws<ArgumentException>(action);
    }
}
