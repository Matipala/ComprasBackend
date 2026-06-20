namespace ComprasBackend.Domain.Entities;

public class PurchaseItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PurchaseId { get; set; }
    public Purchase? Purchase { get; set; }
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
