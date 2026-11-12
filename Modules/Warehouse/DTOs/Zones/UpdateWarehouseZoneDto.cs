using System.ComponentModel.DataAnnotations;
namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Zones
{
    public class UpdateWarehouseZoneDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }


    }
}
