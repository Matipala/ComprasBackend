namespace ComprasBackend.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Supplier { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseItem> Items { get; set; } = new();
}
