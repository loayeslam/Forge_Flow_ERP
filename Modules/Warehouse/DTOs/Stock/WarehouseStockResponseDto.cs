using InventoryManagementAPI.Modules.Warehouse.Models;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Stock
{
    public class WarehouseStockResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public StockStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
