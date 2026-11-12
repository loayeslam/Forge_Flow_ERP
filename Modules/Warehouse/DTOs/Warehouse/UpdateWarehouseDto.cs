using System.ComponentModel.DataAnnotations;
namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Warehouse
{
    public class UpdateWarehouseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
    }
}
