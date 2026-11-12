using InventoryManagementAPI.Modules.Warehouse.DTOs.Transfers;
using InventoryManagementAPI.Modules.Warehouse.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Modules.Warehouse.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseTransferController : ControllerBase
    {
        private readonly WarehouseTransferService _warehouseTransferService;

        public WarehouseTransferController(WarehouseTransferService warehouseTransferService)
        {
            _warehouseTransferService = warehouseTransferService;
        }

        [HttpPost("transfers")]
        public async Task<IActionResult> CreateTransfer(CreateWarehouseTransferDto dto)
        {
            var transfer = await _warehouseTransferService.CreateTransferAsync(dto);

            return Ok(transfer);
        }

        [HttpGet("transfers/{id}")]
        public async Task<IActionResult> GetTransferById(int id)
        {
            var transfer = await _warehouseTransferService.GetTransferByIdAsync(id);

            return Ok(transfer);
        }

        [HttpGet("transfers")]
        public async Task<IActionResult> GetAllTransfers()
        {
            var transfers = await _warehouseTransferService.GetAllTransfersAsync();

            return Ok(transfers);
        }

        [HttpPut("transfers/{id}/approve")]
        public async Task<IActionResult> ApproveTransfer(int id)
        {
            var transfer = await _warehouseTransferService.ApproveTransferAsync(id);

            return Ok(transfer);
        }

        [HttpPut("transfers/{id}/dispatch")]
        public async Task<IActionResult> DispatchTransfer(int id)
        {
            var transfer = await _warehouseTransferService.DispatchTransferAsync(id);

            return Ok(transfer);
        }

        [HttpPut("transfers/{id}/receive")]
        public async Task<IActionResult> ReceiveTransfer(int id)
        {
            var transfer = await _warehouseTransferService.ReceiveTransferAsync(id);

            return Ok(transfer);
        }

        [HttpPut("transfers/{id}/cancel")]
        public async Task<IActionResult> CancelTransfer(int id)
        {
            var transfer = await _warehouseTransferService.CancelTransferAsync(id);

            return Ok(transfer);
        }
    }
}
