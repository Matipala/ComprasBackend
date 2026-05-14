namespace ComprasBackend.Application.DTOs;

public class CreatePurchaseItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int AlmacenId { get; set; }
    public int EmpresaId { get; set; }
}
