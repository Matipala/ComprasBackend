using ComprasBackend.Application.DTOs;

namespace ComprasBackend.Application.Interface;

public interface IPurchaseService
{
    Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto request);
    Task<PurchaseOrderDetailDto?> GetByIdAsync(string orderCen);
    Task<PagedResultDtoOfPurchaseOrderListDto> GetAllAsync(string companyCen, string? status, int page, int pageSize, bool sortDescending);
    Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen);
    Task<List<SupplierDto>> GetSuppliersAsync(string companyCen);
}
