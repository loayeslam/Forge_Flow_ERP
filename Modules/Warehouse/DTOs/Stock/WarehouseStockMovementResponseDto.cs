using InventoryManagementAPI.Modules.Warehouse.Models;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Stock
{
    public class WarehouseStockMovementResponseDto
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public MovementType MovementType { get; set; }
        public int QuantityBefore { get; set; }
        public int QuantityChanged { get; set; }
        public int QuantityAfter { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
