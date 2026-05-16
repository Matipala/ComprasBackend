namespace ComprasBackend.Application.DTOs;

public class CreatePurchaseRequest
{
    public string Supplier { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public List<CreatePurchaseItemRequest> Items { get; set; } = new();
}
