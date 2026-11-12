using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseStockMovement
    {
        public int Id { get; set; }

        [Required]
        public int StockId { get; set; }

        public WarehouseStock Stock { get; set; } = null!;

        public MovementType MovementType { get; set; }

        public int QuantityBefore { get; set; }
        public int QuantityChanged { get; set; }
        public int QuantityAfter { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;


    }
}
