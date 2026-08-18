using CashFlow.Application.DTOs;
using CashFlow.Application.Services;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LancamentosController(ILancamentoService lancamentoService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LancamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CriarLancamentoRequest request, CancellationToken cancellationToken)
    {
        var response = await lancamentoService.RegistrarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Post), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<LancamentoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] Guid? id,
        [FromQuery] Guid? usuarioId,
        [FromQuery] ModalidadeLancamento? tipo,
        CancellationToken cancellationToken)
    {
        var lancamentos = await lancamentoService.BuscarAsync(id, usuarioId, tipo, cancellationToken);
        return Ok(lancamentos);
    }
}
