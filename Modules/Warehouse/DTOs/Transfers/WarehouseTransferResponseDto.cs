namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Transfers
{
    public class WarehouseTransferResponseDto
    {
        public int Id { get; set; }

        public int SourceLocationId { get; set; }

        public int DestinationLocationId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public List<WarehouseTransferItemResponseDto> Items { get; set; } = new List<WarehouseTransferItemResponseDto>();
    }

    public class WarehouseTransferItemResponseDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
