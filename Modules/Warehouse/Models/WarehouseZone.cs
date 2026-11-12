using System.ComponentModel.DataAnnotations;
namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseZone
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; } = null!;

        public ICollection<WarehouseLocation> Locations { get; set; } = new List<WarehouseLocation>();
    }

}
