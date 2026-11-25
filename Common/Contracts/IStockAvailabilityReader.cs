namespace InventoryManagementAPI.Common.Contracts
{
    public interface IStockAvailabilityReader
    {
        Task<Dictionary<int, int>> GetAvailableQuantitiesAsync(IReadOnlyCollection<int> productIds);
    }
}
