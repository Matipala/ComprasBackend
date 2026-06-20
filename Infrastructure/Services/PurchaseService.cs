using ComprasBackend.Application.DTOs;
using ComprasBackend.Application.Interface;
using ComprasBackend.Domain.Entities;
using ComprasBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComprasBackend.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly ComprasDbContext _db;
    private readonly IInventoryClient _inventoryClient;

    public PurchaseService(ComprasDbContext db, IInventoryClient inventoryClient)
    {
        _db = db;
        _inventoryClient = inventoryClient;
    }

    public async Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto request)
    {
        if (string.IsNullOrWhiteSpace(request.SupplierCen))
            throw new InvalidOperationException("SupplierCen es obligatorio");

        if (request.Items == null || request.Items.Count == 0)
            throw new InvalidOperationException("Debe incluir al menos un producto");

        if (request.Items.Any(i => i.Quantity <= 0))
            throw new InvalidOperationException("Las cantidades deben ser mayores a cero");

        var purchase = new Purchase
        {
            SupplierCen = request.SupplierCen.Trim(),
            WarehouseCen = request.WarehouseCen,
            Date = DateTime.UtcNow,
            Status = PurchaseStatus.Pending,
            Items = request.Items.Select(i => new PurchaseItem
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity,
            }).ToList()
        };

        _db.Purchases.Add(purchase);
        await _db.SaveChangesAsync();

        return new PurchaseOrderSummaryDto
        {
            OrderCen = purchase.Id.ToString(),
            Status = purchase.Status
        };
    }

    public async Task<PurchaseOrderDetailDto?> GetByIdAsync(string orderCen)
    {
        if (!Guid.TryParse(orderCen, out var id))
            return null;

        var purchase = await _db.Purchases
            .AsNoTracking()
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (purchase == null) return null;

        return new PurchaseOrderDetailDto
        {
            OrderCen = purchase.Id.ToString(),
            Status = purchase.Status,
            CreatedAt = purchase.Date,
            ConfirmedAt = purchase.ConfirmedAt,
            SupplierCen = purchase.SupplierCen,
            WarehouseCen = purchase.WarehouseCen,
            Items = purchase.Items.Select(i => new PurchaseOrderDetailItemDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task<PagedResultDtoOfPurchaseOrderListDto> GetAllAsync(
        string companyCen, string? status, int page, int pageSize, bool sortDescending)
    {
        var query = _db.Purchases.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => p.Status == status);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        if (totalPages < 1) totalPages = 1;

        IOrderedQueryable<Purchase> ordered = sortDescending
            ? query.OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
            : query.OrderBy(p => p.Date).ThenBy(p => p.Id);

        var items = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PurchaseOrderListDto
            {
                OrderCen = p.Id.ToString(),
                Status = p.Status,
                CreatedAt = p.Date,
                ConfirmedAt = p.ConfirmedAt,
                SupplierCen = p.SupplierCen,
                ItemCount = p.Items.Count
            })
            .ToListAsync();

        return new PagedResultDtoOfPurchaseOrderListDto
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages,
            CurrentPage = page
        };
    }

    public async Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen)
    {
        if (!Guid.TryParse(orderCen, out var id))
            throw new InvalidOperationException("CEN invalido");

        var purchase = await _db.Purchases
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (purchase == null)
            throw new InvalidOperationException("Compra no encontrada");

        if (!string.Equals(purchase.Status, PurchaseStatus.Pending, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo se pueden confirmar compras en estado Pending");

        if (purchase.Items.Any())
        {
            var reason = $"Compra confirmada: ID {purchase.Id}";
            var lines = purchase.Items.Select(i => new PurchaseEntryLineDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity,
                UnitCost = 0
            }).ToList();

            try
            {
                var success = await _inventoryClient.RegisterPurchaseEntryAsync(companyCen, purchase.WarehouseCen, reason, lines);
                if (!success)
                {
                    throw new InvalidOperationException("No se pudo registrar la entrada en el inventario luego de múltiples intentos.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al registrar inventario: {ex.Message}");
            }
        }

        purchase.Status = PurchaseStatus.Confirmed;
        purchase.ConfirmedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new PurchaseOrderConfirmationDto
        {
            OrderCen = purchase.Id.ToString(),
            Status = purchase.Status,
            ConfirmedAt = purchase.ConfirmedAt.Value
        };
    }

    public async Task<List<SupplierDto>> GetSuppliersAsync(string companyCen)
    {
        return await _db.Suppliers
            .AsNoTracking()
            .Where(s => s.IsActive)
            .Select(s => new SupplierDto
            {
                SupplierCen = s.Cen,
                Name = s.Name
            })
            .ToListAsync();
    }

    public static class PurchaseStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Cancelled = "Cancelled";
    }
}
