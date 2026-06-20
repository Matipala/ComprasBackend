namespace ComprasBackend.Application.DTOs;

public class PurchaseOrderListDto
{
    public string OrderCen { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public string SupplierCen { get; set; } = string.Empty;
    public int ItemCount { get; set; }
}

public class PagedResultDtoOfPurchaseOrderListDto
{
    public List<PurchaseOrderListDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}

public class PurchaseOrderDetailDto
{
    public string OrderCen { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public List<PurchaseOrderDetailItemDto> Items { get; set; } = new();
}

public class PurchaseOrderDetailItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class PurchaseOrderSummaryDto
{
    public string OrderCen { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class PurchaseOrderConfirmationDto
{
    public string OrderCen { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ConfirmedAt { get; set; }
}

public class CreatePurchaseOrderDto
{
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}

public class CreatePurchaseOrderItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class SupplierDto
{
    public string SupplierCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
