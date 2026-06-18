namespace ComprasBackend.Domain.Entities;

public class PurchaseItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PurchaseId { get; set; }
    public Purchase? Purchase { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid AlmacenId { get; set; }
    public Guid EmpresaId { get; set; }
}
