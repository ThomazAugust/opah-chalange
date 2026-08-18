using CashFlow.API;
using CashFlow.Domain.Interfaces;
using CashFlow.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace CashFlow.IntegrationTests;

/// <summary>
/// Hospeda a CashFlow.API em memória substituindo os repositórios (banco de dados) por dublês do NSubstitute.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ILancamentoRepository LancamentoRepository { get; } = Substitute.For<ILancamentoRepository>();
    public ISaldoConsolidadoRepository SaldoConsolidadoRepository { get; } = Substitute.For<ISaldoConsolidadoRepository>();

    // Endereço real (Kestrel) da API, necessário para o WireMock conseguir fazer proxy via socket TCP real.
    public Uri ServerAddress { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseUrls("http://127.0.0.1:0");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ConnectionFactory>();
            services.RemoveAll<ILancamentoRepository>();
            services.RemoveAll<ISaldoConsolidadoRepository>();

            services.AddSingleton(LancamentoRepository);
            services.AddSingleton(SaldoConsolidadoRepository);
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // TestServer (necessário pela infraestrutura do WebApplicationFactory) + Kestrel real (necessário para o proxy do WireMock).
        var testHost = builder.Build();

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
        var kestrelHost = builder.Build();

        testHost.Start();
        kestrelHost.Start();

        var server = kestrelHost.Services.GetRequiredService<IServer>();
        var addressesFeature = server.Features.Get<IServerAddressesFeature>();
        ServerAddress = new Uri(addressesFeature!.Addresses.First());

        return testHost;
    }
}
