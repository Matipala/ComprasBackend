namespace ComprasBackend.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseItem> Items { get; set; } = new();
}
