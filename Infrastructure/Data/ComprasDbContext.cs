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
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("compras");

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.SupplierCen).IsRequired();
            entity.Property(e => e.WarehouseCen).IsRequired();
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
            entity.Property(e => e.ProductCen).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers", "compras");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Cen).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Cen).IsUnique();

            entity.HasData(
                new Supplier { Id = Guid.Parse("a0000000-0000-0000-0000-000000000001"), Cen = "prov-001", Name = "Distribuidora Nacional S.A." },
                new Supplier { Id = Guid.Parse("a0000000-0000-0000-0000-000000000002"), Cen = "prov-002", Name = "Importadora Del Sur Ltda." },
                new Supplier { Id = Guid.Parse("a0000000-0000-0000-0000-000000000003"), Cen = "prov-003", Name = "Mayorista El Buen Precio" },
                new Supplier { Id = Guid.Parse("a0000000-0000-0000-0000-000000000004"), Cen = "prov-004", Name = "Productos Frescos del Campo" },
                new Supplier { Id = Guid.Parse("a0000000-0000-0000-0000-000000000005"), Cen = "prov-005", Name = "Bebidas y Licores Premium" }
            );
        });
    }
}
