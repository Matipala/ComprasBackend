using Microsoft.EntityFrameworkCore;
using ComprasBackend.Application.Interface;
using ComprasBackend.Application.Services;
using ComprasBackend.Infrastructure.Data;
using ComprasBackend.Infrastructure.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddDbContext<ComprasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    var baseUrl = builder.Configuration["InventoryApi:BaseUrl"] ?? "http://localhost:5143";
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

// if (!app.Environment.IsDevelopment())
// {
//     app.UseHttpsRedirection();
// }

app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "compras" }));

app.Run();
