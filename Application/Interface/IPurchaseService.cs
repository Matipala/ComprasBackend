using ComprasBackend.Application.DTOs;

namespace ComprasBackend.Application.Interface;

public interface IPurchaseService
{
    Task<PurchaseDto> CreateAsync(CreatePurchaseRequest request);
    Task<PurchaseDto?> GetByIdAsync(int id);
    Task<List<PurchaseDto>> GetAllAsync();
    Task<PurchaseDto> ConfirmAsync(int id);
}
