using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseTransfer
    {
        public int Id { get; set; }

        [Required]
        public int SourceLocationId { get; set; }

        public WarehouseLocation SourceLocation { get; set; } = null!;

        [Required]
        public int DestinationLocationId { get; set; }

        public WarehouseLocation DestinationLocation { get; set; } = null!;

        public TransferStatus Status { get; set; } = TransferStatus.Pending;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        public ICollection<WarehouseTransferItem> Items { get; set; } = new List<WarehouseTransferItem>();
    }
}