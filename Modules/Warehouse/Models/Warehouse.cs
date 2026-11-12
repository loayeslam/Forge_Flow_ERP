using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<WarehouseZone> WarehouseZones { get; set; } = new List<WarehouseZone>();
    }
}
