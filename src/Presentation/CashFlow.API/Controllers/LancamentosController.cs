using CashFlow.Application.DTOs;
using CashFlow.Application.Services;
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
}
