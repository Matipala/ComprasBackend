using ComprasBackend.Application.DTOs;

namespace ComprasBackend.Application.Interface;

public interface IPurchaseService
{
    Task<PurchaseDto> CreateAsync(CreatePurchaseRequest request);
    Task<PurchaseDto?> GetByIdAsync(Guid id);
    Task<List<PurchaseDto>> GetAllAsync();
    Task<PurchaseDto> ConfirmAsync(Guid id);
}
