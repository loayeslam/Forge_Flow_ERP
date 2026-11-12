namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Locations
{
    public class WarehouseLocationResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ZoneId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}
