using System.ComponentModel.DataAnnotations;


namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Stock
{
    public class AdjustWarehouseStockDto
    {
        [Range(0, int.MaxValue)]
        public int PhysicalQuantity { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Reason { get; set; } = string.Empty;
    }
}
