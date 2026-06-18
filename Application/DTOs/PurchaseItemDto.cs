namespace ComprasBackend.Application.DTOs;

public class PurchaseItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid AlmacenId { get; set; }
    public Guid EmpresaId { get; set; }
}
