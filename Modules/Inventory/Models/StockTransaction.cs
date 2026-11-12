namespace InventoryManagementAPI.Modules.Inventory.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public StockTransactionType Type { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}
