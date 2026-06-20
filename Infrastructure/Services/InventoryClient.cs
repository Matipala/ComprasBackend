using System.Net.Http.Json;
using Polly;
using Polly.CircuitBreaker;
using ComprasBackend.Application.Interface;

namespace ComprasBackend.Infrastructure.Services;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy _retryPolicy;
    private readonly IAsyncPolicy _circuitBreaker;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[Polly Retry] Intento {retryCount}/3 tras {timeSpan.TotalSeconds}s - {exception.GetType().Name}");
                });

        _circuitBreaker = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, ts) => Console.WriteLine($"[Polly CircuitBreaker] ABIERTO por {ts.TotalSeconds}s - {ex.Message}"),
                onReset: () => Console.WriteLine("[Polly CircuitBreaker] CERRADO - circuito restaurado"),
                onHalfOpen: () => Console.WriteLine("[Polly CircuitBreaker] SEMI-ABIERTO - probando..."));
    }

    public async Task<bool> RegisterPurchaseEntryAsync(string companyCen, string warehouseCen, string reason, List<PurchaseEntryLineDto> lines)
    {
        var url = $"/api/inventory/companies/{companyCen}/documents";
        Console.WriteLine($"[Compras→Inventario] Llamando a {_httpClient.BaseAddress}{url} (timeout: {_httpClient.Timeout.TotalSeconds}s)");

        var resilience = Policy.WrapAsync(_retryPolicy, _circuitBreaker);

        try
        {
            documentType = "Entrada",
            warehouseCen = warehouseCen,
            reason = reason,
            lines = lines.Select(l => new
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

        HttpResponseMessage response = null;
        for (int i = 0; i < 3; i++)
        {
            try
            {
                response = await _httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/documents", request);
                if (response.IsSuccessStatusCode)
                    return true;
            }
            catch
            {
                // Ignore exception to retry
            }
            await Task.Delay(500 * (i + 1));
        }

        return false;
    }
}
