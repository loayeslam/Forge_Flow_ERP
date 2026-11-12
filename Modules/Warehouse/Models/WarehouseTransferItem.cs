using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Modules.Warehouse.Models
{
    public class WarehouseTransferItem
    {
        public int Id { get; set; }

        [Required]
        public int TransferId { get; set; }

        public WarehouseTransfer Transfer { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}