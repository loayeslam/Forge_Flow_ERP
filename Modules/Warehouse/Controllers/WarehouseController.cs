using InventoryManagementAPI.Modules.Warehouse.DTOs.Warehouse;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Zones;
using InventoryManagementAPI.Modules.Warehouse.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Modules.Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var warehouses = await _warehouseService.GetAllAsync();

            var response = warehouses.Select(w => new WarehouseResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Address = w.Address,
                IsActive = w.IsActive,
                CreatedAt = w.CreatedAt,

                WarehouseZones = w.WarehouseZones
            .Select(z => new WarehouseZoneResponseDto
            {
                Id = z.Id,
                Name = z.Name,
                Description = z.Description,
                WarehouseId = z.WarehouseId
            }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound(new { Message = "Warehouse not found." });
            }
            var response = new WarehouseResponseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Address = warehouse.Address,
                IsActive = warehouse.IsActive,
                CreatedAt = warehouse.CreatedAt,

                WarehouseZones = warehouse.WarehouseZones
            .Select(z => new WarehouseZoneResponseDto
            {
                Id = z.Id,
                Name = z.Name,
                Description = z.Description,
                WarehouseId = z.WarehouseId
            }).ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarehouseDto dto)
        {
            var warehouse = await _warehouseService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, warehouse);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateWarehouseDto dto)
        {
            var warehouse = await _warehouseService.UpdateAsync(id, dto);
            if (warehouse == null)
            {
                return NotFound(new { Message = "Warehouse not found." });
            }
            return Ok(warehouse);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateWarehouseStatusDto dto)
        {
            var warehouse = await _warehouseService.UpdateStatusAsync(id, dto);
            if (warehouse == null)
            {
                return NotFound(new { Message = "Warehouse not found." });
            }
            return Ok(warehouse);
        }
        

    }
}
