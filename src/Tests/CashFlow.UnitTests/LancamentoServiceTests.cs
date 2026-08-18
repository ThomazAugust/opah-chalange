using CashFlow.Application.Services;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;

namespace CashFlow.UnitTests;

public class LancamentoServiceTests
{
    private static readonly Guid UsuarioA = Guid.NewGuid();
    private static readonly Guid UsuarioB = Guid.NewGuid();

    private static LancamentoService CriarService(out FakeLancamentoRepository repository)
    {
        repository = new FakeLancamentoRepository(
        [
            new Lancamento(Guid.NewGuid(), "Recebimento", 100m, ModalidadeLancamento.Credito, DateTimeOffset.UtcNow, UsuarioA),
            new Lancamento(Guid.NewGuid(), "Pagamento", 50m, ModalidadeLancamento.Debito, DateTimeOffset.UtcNow, UsuarioB)
        ]);

        return new LancamentoService(repository, NullLogger<LancamentoService>.Instance);
    }

    [Fact]
    public async Task Deve_Buscar_Lancamento_Por_Id()
    {
        var service = CriarService(out var repository);
        var esperado = repository.Lancamentos[0];

        var resultado = await service.BuscarAsync(esperado.Id, usuarioId: null, tipo: null);

        var lancamento = Assert.Single(resultado);
        Assert.Equal(esperado.Id, lancamento.Id);
    }

    [Fact]
    public async Task Deve_Buscar_Lancamentos_Por_UsuarioId()
    {
        var service = CriarService(out _);

        var resultado = await service.BuscarAsync(id: null, usuarioId: UsuarioA, tipo: null);

        var lancamento = Assert.Single(resultado);
        Assert.Equal(UsuarioA, lancamento.UsuarioId);
    }

    [Fact]
    public async Task Deve_Buscar_Lancamentos_Por_Tipo()
    {
        var service = CriarService(out _);

        var resultado = await service.BuscarAsync(id: null, usuarioId: null, tipo: ModalidadeLancamento.Debito);

        var lancamento = Assert.Single(resultado);
        Assert.Equal(ModalidadeLancamento.Debito, lancamento.Tipo);
    }

    private sealed class FakeLancamentoRepository(List<Lancamento> lancamentos) : ILancamentoRepository
    {
        public List<Lancamento> Lancamentos { get; } = lancamentos;

        public Task<Guid> AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default)
        {
            Lancamentos.Add(lancamento);
            return Task.FromResult(lancamento.Id);
        }

        public Task<IReadOnlyCollection<Lancamento>> ListByDateAsync(DateOnly data, CancellationToken cancellationToken = default)
        {
            var resultado = Lancamentos.Where(l => DateOnly.FromDateTime(l.DataLancamento.UtcDateTime) == data).ToArray();
            return Task.FromResult<IReadOnlyCollection<Lancamento>>(resultado);
        }

        public Task<IReadOnlyCollection<Lancamento>> BuscarAsync(Guid? id, Guid? usuarioId, ModalidadeLancamento? tipo, CancellationToken cancellationToken = default)
        {
            var resultado = Lancamentos
                .Where(l => !id.HasValue || l.Id == id.Value)
                .Where(l => !usuarioId.HasValue || l.UsuarioId == usuarioId.Value)
                .Where(l => !tipo.HasValue || l.Tipo == tipo.Value)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<Lancamento>>(resultado);
        }
    }
}
