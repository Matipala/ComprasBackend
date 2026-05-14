using ComprasBackend.Application.DTOs;
using ComprasBackend.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComprasBackend.Presentation.Controllers;

[ApiController]
[Route("api/purchases/orders")]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _service;

    public PurchasesController(IPurchaseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseDto>> Create([FromBody] CreatePurchaseRequest request)
    {
        try
        {
            var created = await _service.CreateAsync(request);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseDto>>> GetAll()
    {
        var purchases = await _service.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseDto>> GetById(int id)
    {
        var purchase = await _service.GetByIdAsync(id);
        if (purchase == null)
            return NotFound();

        return Ok(purchase);
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<ActionResult<PurchaseDto>> Confirm(int id)
    {
        try
        {
            var updated = await _service.ConfirmAsync(id);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
