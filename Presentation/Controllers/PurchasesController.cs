using ComprasBackend.Application.DTOs;
using ComprasBackend.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComprasBackend.Presentation.Controllers;

[ApiController]
[Route("api/purchases/companies/{companyCen}")]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _service;

    public PurchasesController(IPurchaseService service)
    {
        _service = service;
    }

    [HttpGet("orders")]
    public async Task<ActionResult<PagedResultDtoOfPurchaseOrderListDto>> GetAll(
        string companyCen,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool sortDescending = true)
    {
        var result = await _service.GetAllAsync(companyCen, status, page, pageSize, sortDescending);
        return Ok(result);
    }

    [HttpPost("orders")]
    public async Task<ActionResult<PurchaseOrderSummaryDto>> Create(string companyCen, [FromBody] CreatePurchaseOrderDto request)
    {
        try
        {
            var created = await _service.CreateAsync(companyCen, request);
            return CreatedAtAction(nameof(GetById), new { companyCen, orderCen = created.OrderCen }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("orders/{orderCen}")]
    public async Task<ActionResult<PurchaseOrderDetailDto>> GetById(string companyCen, string orderCen)
    {
        var purchase = await _service.GetByIdAsync(orderCen);
        if (purchase == null)
            return NotFound();

        return Ok(purchase);
    }

    [HttpPost("orders/{orderCen}/confirm")]
    public async Task<ActionResult<PurchaseOrderConfirmationDto>> Confirm(string companyCen, string orderCen)
    {
        try
        {
            var result = await _service.ConfirmAsync(companyCen, orderCen);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("suppliers")]
    public async Task<ActionResult<List<SupplierDto>>> GetSuppliers(string companyCen)
    {
        var suppliers = await _service.GetSuppliersAsync(companyCen);
        return Ok(suppliers);
    }
}
