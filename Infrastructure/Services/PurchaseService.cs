using ComprasBackend.Application.DTOs;
using ComprasBackend.Application.Interface;
using ComprasBackend.Domain.Entities;
using ComprasBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComprasBackend.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly ComprasDbContext _db;

    public PurchaseService(ComprasDbContext db)
    {
        _db = db;
    }

    public async Task<PurchaseDto> CreateAsync(CreatePurchaseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Supplier))
            throw new InvalidOperationException("Supplier es obligatorio");

        if (request.Items == null || request.Items.Count == 0)
            throw new InvalidOperationException("Debe incluir al menos un producto");

        if (request.Items.Any(i => i.Quantity <= 0))
            throw new InvalidOperationException("Las cantidades deben ser mayores a cero");

        var purchase = new Purchase
        {
            Supplier = request.Supplier.Trim(),
            Date = request.Date ?? DateTime.UtcNow,
            Status = PurchaseStatus.Pending,
            Items = request.Items.Select(i => new PurchaseItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                AlmacenId = i.AlmacenId,
                EmpresaId = i.EmpresaId
            }).ToList()
        };

        _db.Purchases.Add(purchase);
        await _db.SaveChangesAsync();

        return MapToDto(purchase);
    }

    public async Task<PurchaseDto?> GetByIdAsync(int id)
    {
        var purchase = await _db.Purchases
            .AsNoTracking()
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        return purchase == null ? null : MapToDto(purchase);
    }

    public async Task<List<PurchaseDto>> GetAllAsync()
    {
        var purchases = await _db.Purchases
            .AsNoTracking()
            .Include(p => p.Items)
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .ToListAsync();

        return purchases.Select(MapToDto).ToList();
    }

    public async Task<PurchaseDto> ConfirmAsync(int id)
    {
        var purchase = await _db.Purchases
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (purchase == null)
            throw new InvalidOperationException("Compra no encontrada");

        if (!string.Equals(purchase.Status, PurchaseStatus.Pending, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo se pueden confirmar compras en estado Pending");

        purchase.Status = PurchaseStatus.Confirmed;
        await _db.SaveChangesAsync();

        return MapToDto(purchase);
    }

    private static PurchaseDto MapToDto(Purchase purchase)
    {
        return new PurchaseDto
        {
            Id = purchase.Id,
            Supplier = purchase.Supplier,
            Date = purchase.Date,
            Status = purchase.Status,
            Items = purchase.Items.Select(i => new PurchaseItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                AlmacenId = i.AlmacenId,
                EmpresaId = i.EmpresaId
            }).ToList()
        };
    }

    public static class PurchaseStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Cancelled = "Cancelled";
    }
}
