namespace ComprasBackend.Application.DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseItemDto> Items { get; set; } = new();
}
