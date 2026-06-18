using ComprasBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComprasBackend.Infrastructure.Data;

public class ComprasDbContext : DbContext
{
    public ComprasDbContext(DbContextOptions<ComprasDbContext> options) : base(options)
    {
    }

    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("compras");

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Supplier).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.HasMany(e => e.Items)
                .WithOne(i => i.Purchase)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.AlmacenId).IsRequired();
            entity.Property(e => e.EmpresaId).IsRequired();
        });
    }
}
