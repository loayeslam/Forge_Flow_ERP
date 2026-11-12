using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseLocation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int ZoneId { get; set; }

        public WarehouseZone Zone { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;


        public ICollection<WarehouseStock> Stocks { get; set; } = new List<WarehouseStock>();
    }
}
