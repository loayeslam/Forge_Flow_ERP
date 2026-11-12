using InventoryManagementAPI.Modules.Warehouse.DTOs.Locations;
using InventoryManagementAPI.Modules.Warehouse.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Modules.Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseLocationController : ControllerBase
    {
        private readonly WarehouseLocationService _warehouseLocationService;
        private readonly WarehouseZoneService _warehouseZoneService;

        public WarehouseLocationController(WarehouseLocationService warehouseLocationService, WarehouseZoneService warehouseZoneService)
        {
            _warehouseLocationService = warehouseLocationService;
            _warehouseZoneService = warehouseZoneService;
        }
        [HttpPost("locations")]
        public async Task<IActionResult> CreateLocation(CreateWarehouseLocationDto dto)
        {
            var location = await _warehouseLocationService.CreateLocationAsync(dto);

            return CreatedAtAction(
                nameof(GetLocationById),
                new { id = location.Id },
                location
            );
        }

        [HttpGet("locations/{id:int}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _warehouseLocationService.GetLocationByIdAsync(id);

            if (location == null)
                return NotFound(new { Message = "Location not found." });

            return Ok(location);
        }

        [HttpGet("zones/{zoneId:int}/locations")]
        public async Task<IActionResult> GetLocationsByZone(int zoneId)
        {
            var zone = await _warehouseZoneService.GetZoneByIdAsync(zoneId);

            if (zone == null)
            {
                return NotFound(new { Message = "Zone not found. " });
            }

            var locations = await _warehouseLocationService.GetLocationsByZoneIdAsync(zoneId);


            return Ok(locations);
        }

        [HttpPut("locations/{id:int}")]
        public async Task<IActionResult> UpdateLocation(int id, UpdateWarehouseLocationDto dto)
        {
            var location = await _warehouseLocationService.UpdateLocationAsync(id, dto);
            if (location == null)
            {
                return NotFound(new { Message = "Location not found." });
            }
            return Ok(location);
        }

        [HttpPatch("locations/{id:int}/status")]
        public async Task<IActionResult> UpdateLocationStatus(int id, UpdateWarehouseLocationStatusDto dto)
        {
            var location = await _warehouseLocationService.UpdateLocationStatusAsync(id, dto);
            if (location == null)
            {
                return NotFound(new { Message = "Location not found." });

            }
            return Ok(location);
        }

        [HttpDelete("locations/{id:int}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var deleted = await _warehouseLocationService.DeleteLocationAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Location not found." });
            }

            return NoContent();

        }


    }
}
