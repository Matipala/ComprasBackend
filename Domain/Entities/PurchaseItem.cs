namespace ComprasBackend.Domain.Entities;

public class PurchaseItem
{
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public Purchase? Purchase { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int AlmacenId { get; set; }
    public int EmpresaId { get; set; }
}
