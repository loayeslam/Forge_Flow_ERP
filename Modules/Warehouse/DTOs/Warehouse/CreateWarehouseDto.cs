using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Warehouse
{
    public class CreateWarehouseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
    }
}
