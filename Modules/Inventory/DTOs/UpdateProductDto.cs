using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace InventoryManagementAPI.Modules.Inventory.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; }
        [Range(typeof(decimal), "0.01", "9999999999999")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int MinimumStockLevel { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }



    }
}
