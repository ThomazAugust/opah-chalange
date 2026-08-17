using CashFlow.Application.DTOs;
using CashFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consolidation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsolidacaoController(ISaldoConsolidadoService saldoConsolidadoService) : ControllerBase
{
    [HttpGet("{data}")]
    [ProducesResponseType(typeof(SaldoDiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] DateOnly data, CancellationToken cancellationToken)
    {
        var saldo = await saldoConsolidadoService.ObterPorDataAsync(data, cancellationToken);
        if (saldo is null)
        {
            return Ok(null);
        }

        return Ok(saldo);
    }

    [HttpPost("reprocessar/{data}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Reprocessar([FromRoute] DateOnly data, CancellationToken cancellationToken)
    {
        await saldoConsolidadoService.ReprocessarDiaAsync(data, cancellationToken);
        return Accepted();
    }
}
