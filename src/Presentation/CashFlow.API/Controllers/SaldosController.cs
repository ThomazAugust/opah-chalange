using CashFlow.Application.DTOs;
using CashFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaldosController(ISaldoConsolidadoService saldoConsolidadoService) : ControllerBase
{
    [HttpGet("{data}")]
    [ProducesResponseType(typeof(SaldoDiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDate([FromRoute] DateOnly data, CancellationToken cancellationToken)
    {
        var saldo = await saldoConsolidadoService.ObterPorDataAsync(data, cancellationToken);
        if (saldo is null)
        {
            return NotFound();
        }

        return Ok(saldo);
    }
}
