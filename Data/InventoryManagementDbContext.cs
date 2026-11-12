using InventoryManagementAPI.Modules.Inventory.Models;
using InventoryManagementAPI.Modules.Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class InventoryManagementDbContext : DbContext
    {

        public InventoryManagementDbContext(DbContextOptions<InventoryManagementDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<WarehouseZone> WarehouseZones { get; set; }

        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }

        public DbSet<WarehouseStock> WarehouseStocks { get; set; }

        public DbSet<WarehouseStockMovement> WarehouseStockMovements { get; set; }

        public DbSet<WarehouseTransfer> WarehouseTransfers { get; set; }

        public DbSet<WarehouseTransferItem> WarehouseTransferItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WarehouseStock>()
                .HasIndex(s => new
                {
                    s.ProductId,
                    s.LocationId,
                    s.Status
                })
                .IsUnique();

            modelBuilder.Entity<WarehouseStockMovement>()
                .HasOne(m => m.Stock)
                .WithMany(s => s.Movements)
                .HasForeignKey(m => m.StockId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<WarehouseTransfer>()
                .HasOne(t => t.SourceLocation)
                .WithMany()
                .HasForeignKey(t => t.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseTransfer>()
                .HasOne(t => t.DestinationLocation)
                .WithMany()
                .HasForeignKey(t => t.DestinationLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseTransferItem>()
                .HasOne(i => i.Transfer)
                .WithMany(t => t.Items)
                .HasForeignKey(i => i.TransferId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WarehouseStock>()
                .ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_WarehouseStocks_Quantity",
                        "[Quantity] >= 0");

                    table.HasCheckConstraint(
                        "CK_WarehouseStocks_ReservedQuantity",
                        "[ReservedQuantity] >= 0");

                    table.HasCheckConstraint(
                        "CK_WarehouseStocks_ReservedQuantity_LessThan_Quantity",
                        "[ReservedQuantity] <= [Quantity]");
                });
        }
    

    }
}
