using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Inventory.DTOs
{
    public class StockTransactionDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; } 
    }
}
