using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Stock
{
    public class IssueWarehouseStockDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Reason { get; set; } = string.Empty;
    }

}
