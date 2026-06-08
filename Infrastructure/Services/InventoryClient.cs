using System.Net.Http.Json;
using ComprasBackend.Application.Interface;
using Microsoft.Extensions.Configuration;

namespace ComprasBackend.Infrastructure.Services;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public InventoryClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["InventoryApi:BaseUrl"] ?? "http://localhost:5143";
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

        var url = $"{_baseUrl}/api/inventory/companies/{companyCen}/documents";
        var response = await _httpClient.PostAsJsonAsync(url, request);

        return response.IsSuccessStatusCode;
    }
}
