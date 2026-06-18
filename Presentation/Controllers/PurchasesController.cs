using ComprasBackend.Application.DTOs;
using ComprasBackend.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComprasBackend.Presentation.Controllers;

[ApiController]
[Route("api/purchases/companies/{companyCen}/orders")]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _service;

    public PurchasesController(IPurchaseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseDto>> Create(string companyCen, [FromBody] CreatePurchaseRequest request)
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
    public async Task<ActionResult<List<PurchaseDto>>> GetAll(string companyCen)
    {
        var purchases = await _service.GetAllAsync();
        return Ok(purchases);
    }

    private Guid ResolveOrderId(string orderCen)
    {
        if (Guid.TryParse(orderCen, out Guid id)) return id;
        return Guid.Empty; 
    }

    [HttpGet("{orderCen}")]
    public async Task<ActionResult<PurchaseDto>> GetById(string companyCen, string orderCen)
    {
        var id = ResolveOrderId(orderCen);
        var purchase = await _service.GetByIdAsync(id);
        if (purchase == null)
            return NotFound();

        return Ok(purchase);
    }

    [HttpPost("{orderCen}/confirm")]
    public async Task<ActionResult<PurchaseDto>> Confirm(string companyCen, string orderCen)
    {
        try
        {
            var id = ResolveOrderId(orderCen);
            var updated = await _service.ConfirmAsync(id);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
