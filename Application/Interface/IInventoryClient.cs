namespace ComprasBackend.Application.Interface;

public interface IInventoryClient
{
    Task<bool> RegisterPurchaseEntryAsync(string companyCen, string warehouseCen, string reason, List<PurchaseEntryLineDto> lines);
}

public class PurchaseEntryLineDto
{
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
