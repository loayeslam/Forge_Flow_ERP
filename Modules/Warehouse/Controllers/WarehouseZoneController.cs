using InventoryManagementAPI.Modules.Warehouse.DTOs.Zones;
using InventoryManagementAPI.Modules.Warehouse.Services;
using Microsoft.AspNetCore.Mvc;


namespace InventoryManagementAPI.Modules.Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseZoneController : ControllerBase
    {
        private readonly WarehouseZoneService _warehouseZoneService;
        private readonly WarehouseService _warehouseService;

        public WarehouseZoneController(WarehouseZoneService warehouseZoneService, WarehouseService warehouseService)
        {
            _warehouseZoneService = warehouseZoneService;
            _warehouseService = warehouseService;
        }
        [HttpPost("zones")]
        public async Task<IActionResult> CreateZone(CreateWarehouseZoneDto dto)
        {
            var zone = await _warehouseZoneService.CreateZoneAsync(dto);

            return CreatedAtAction( nameof(GetZoneById), new { zoneId = zone.Id }, zone);
        }

        [HttpGet("{id:int}/zones")]
        public async Task<IActionResult> GetZones(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound(new { Message = "Warehouse not found." });
            }

            var zones = await _warehouseZoneService.GetZonesByWarehouseIdAsync(id);
            return Ok(zones);
        }

        [HttpGet("zones/{zoneId:int}")]
        public async Task<IActionResult> GetZoneById(int zoneId)
        {
            var zone = await _warehouseZoneService.GetZoneByIdAsync(zoneId);

            if (zone == null)
            {
                return NotFound(new { Message = "Zone not found. " });
            }

            return Ok(zone);

        }

        [HttpPut("zones/{zoneId:int}")]
        public async Task<IActionResult> UpdarteZones(int zoneId, UpdateWarehouseZoneDto dto)
        {
            var zone = await _warehouseZoneService.UpdateZoneAsync(zoneId, dto);
            if (zone == null)
            {
                return NotFound(new { Message = "Zone not found." });
            }
            return Ok(zone);
        }

        [HttpDelete("zones/{zoneId:int}")]
        public async Task<IActionResult> DeleteZone(int zoneId)
        {
            var deleted = await _warehouseZoneService.DeleteZoneAsync(zoneId);
            if (!deleted)
            {
                return NotFound(new { Message = "Zone not found. " });
            }
            return NoContent();
        }
    }
}
