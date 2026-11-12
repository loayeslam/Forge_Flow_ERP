using System.ComponentModel.DataAnnotations;
namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Locations
{
    public class CreateWarehouseLocationDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ZoneId { get; set; }


    }
}
