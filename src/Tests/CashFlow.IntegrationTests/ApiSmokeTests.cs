using System.Net.Http.Headers;
using System.Net.Http.Json;
using CashFlow.API.Controllers;
using CashFlow.Application.DTOs;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using NSubstitute;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace CashFlow.IntegrationTests;

public class ApiSmokeTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private HttpClient _testServerClient = null!;
    private WireMockServer _wireMockServer = null!;
    private HttpClient _wireMockClient = null!;

    public ApiSmokeTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public Task InitializeAsync()
    {
        _ = _factory.Services; // força a inicialização lazy do host (Kestrel) para termos o ServerAddress real

        // HttpClient real (socket TCP), necessário pois o proxy do WireMock não alcança o TestServer em memória.
        _testServerClient = new HttpClient { BaseAddress = _factory.ServerAddress };

        // WireMock hospeda o "endpoint público" e repassa (proxy) para a API real do TestServer.
        _wireMockServer = WireMockServer.Start();
        _wireMockServer
            .Given(Request.Create().WithPath("/*").UsingAnyMethod())
            .RespondWith(Response.Create().WithProxy(new ProxyAndRecordSettings
            {
                Url = _factory.ServerAddress.ToString().TrimEnd('/')
            }));

        _wireMockClient = new HttpClient { BaseAddress = new Uri(_wireMockServer.Url!) };

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _wireMockClient.Dispose();
        _wireMockServer.Stop();
        _wireMockServer.Dispose();
        _testServerClient.Dispose();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Deve_Buscar_Lancamentos_Via_WireMock_Com_Repositorio_Mockado()
    {
        var usuarioId = Guid.NewGuid();
        var lancamentoId = Guid.NewGuid();

        _factory.LancamentoRepository
            .BuscarAsync(Arg.Any<Guid?>(), usuarioId, Arg.Any<ModalidadeLancamento?>(), Arg.Any<CancellationToken>())
            .Returns(new List<Lancamento>
            {
                new(lancamentoId, "Recebimento simulado", 200m, ModalidadeLancamento.Credito, DateTimeOffset.UtcNow, usuarioId)
            });

        var token = await ObterTokenAsync(usuarioId);
        _wireMockClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _wireMockClient.GetAsync($"/api/lancamentos?usuarioId={usuarioId}");
        response.EnsureSuccessStatusCode();

        var lancamentos = await response.Content.ReadFromJsonAsync<List<LancamentoResponse>>();

        var lancamento = Assert.Single(lancamentos!);
        Assert.Equal(lancamentoId, lancamento.Id);
        Assert.Equal(usuarioId, lancamento.UsuarioId);
    }

    private async Task<string> ObterTokenAsync(Guid usuarioId)
    {
        var response = await _testServerClient.PostAsJsonAsync("/api/auth/login", new LoginRequest { UsuarioId = usuarioId });
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return payload!.Token;
    }

    private sealed record LoginResponse(string Token);
}
