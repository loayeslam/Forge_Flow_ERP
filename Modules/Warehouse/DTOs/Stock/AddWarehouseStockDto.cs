using System.ComponentModel.DataAnnotations;
using InventoryManagementAPI.Modules.Warehouse.Models;
namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Stock
{
    public class AddWarehouseStockDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int LocationId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public StockStatus Status { get; set; } = StockStatus.Available;

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Reason { get; set; } = string.Empty;
    }
}
