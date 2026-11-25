namespace InventoryManagementAPI.Modules.Inventory.DTOs
{
    public class LowStockProductDto
    {
        public int ProductId {  get; set; }
        public string? Name { get; set; }
        public int MinimumStockLevel { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
