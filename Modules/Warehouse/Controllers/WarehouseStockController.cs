using InventoryManagementAPI.Modules.Warehouse.DTOs.Stock;
using InventoryManagementAPI.Modules.Warehouse.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Modules.Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseStockController : ControllerBase
    {
        private readonly WarehouseStockService _warehouseStockService;

        public WarehouseStockController(WarehouseStockService warehouseStockService)
        {
            _warehouseStockService = warehouseStockService;
        }

        [HttpPost("stock")]
        public async Task<IActionResult> AddStock(AddWarehouseStockDto dto)
        {
            var stock = await _warehouseStockService.AddStockAsync(dto);

            return Ok(stock);
        }

        [HttpGet("locations/{locationId}/stock")]
        public async Task<IActionResult> GetStockByLocation(int locationId)
        {
            var stocks = await _warehouseStockService.GetStockByLocationAsync(locationId);

            return Ok(stocks);
        }

        [HttpGet("products/{productId}/stock")]
        public async Task<IActionResult> GetStockByProduct(int productId)
        {
            var stocks = await _warehouseStockService.GetStockByProductAsync(productId);

            return Ok(stocks);
        }

        [HttpGet("{warehouseId}/stock")]
        public async Task<IActionResult> GetStockByWarehouse(int warehouseId)
        {
            var stocks = await _warehouseStockService.GetStockByWarehouseAsync(warehouseId);

            return Ok(stocks);
        }

        [HttpPost("stock/{stockId}/issue")]
        public async Task<IActionResult> IssueStock(int stockId, IssueWarehouseStockDto dto)
        {
            var stock = await _warehouseStockService.IssueStockAsync(stockId, dto);

            return Ok(stock);
        }

        [HttpPost("stock/{stockId}/adjust")]
        public async Task<IActionResult> AdjustStock(int stockId, AdjustWarehouseStockDto dto)
        {
            var stock = await _warehouseStockService.AdjustStockAsync(stockId, dto);

            return Ok(stock);
        }

        [HttpGet("stock/{stockId}/movements")]
        public async Task<IActionResult> GetStockMovements(int stockId)
        {
            var movements = await _warehouseStockService.GetStockMovementsAsync(stockId);

            return Ok(movements);
        }

    }
}
