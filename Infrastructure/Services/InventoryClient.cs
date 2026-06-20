using System.Net.Http.Json;
using ComprasBackend.Application.Interface;

namespace ComprasBackend.Infrastructure.Services;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RegisterPurchaseEntryAsync(string companyCen, string warehouseCen, string reason, List<PurchaseEntryLineDto> lines)
    {
        var request = new
        {
            documentType = "ENTRY",
            warehouseCen = warehouseCen,
            reason = reason,
            lines = lines.Select(l => new
            {
                productCen = l.ProductCen,
                quantity = l.Quantity,
                unitCost = l.UnitCost
            })
        };

        var response = await _httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/documents", request);

        return response.IsSuccessStatusCode;
    }
}
