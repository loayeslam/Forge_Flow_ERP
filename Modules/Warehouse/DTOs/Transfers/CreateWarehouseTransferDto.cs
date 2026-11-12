using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.DTOs.Transfers
{
    public class CreateWarehouseTransferDto
    {
        [Range(1, int.MaxValue)]
        public int SourceLocationId { get; set; }

        [Range(1, int.MaxValue)]
        public int DestinationLocationId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateWarehouseTransferItemDto> Items { get; set; } = new List<CreateWarehouseTransferItemDto>();

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class CreateWarehouseTransferItemDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
