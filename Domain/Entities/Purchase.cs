namespace ComprasBackend.Domain.Entities;

public class Purchase
{
    public int Id { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseItem> Items { get; set; } = new();
}
