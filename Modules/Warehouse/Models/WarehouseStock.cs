using System.ComponentModel.DataAnnotations;
namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public WarehouseLocation Location { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReservedQuantity { get; set; }

        public StockStatus Status { get; set; } = StockStatus.Available;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;


        public ICollection<WarehouseStockMovement> Movements { get; set; } = new List<WarehouseStockMovement>();
    }
}
