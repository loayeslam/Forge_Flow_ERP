using InventoryManagementAPI.Modules.Warehouse.DTOs.Zones;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Warehouse
{
    public class WarehouseResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ICollection<WarehouseZoneResponseDto> WarehouseZones { get; set; }
            = new List<WarehouseZoneResponseDto>();
    }
}
