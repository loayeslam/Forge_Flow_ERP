
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Zones;

namespace InventoryManagementAPI.Modules.Warehouse.Services
{
    public class WarehouseZoneService
    {
        private readonly InventoryManagementDbContext _context;

        public WarehouseZoneService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseZoneResponseDto> CreateZoneAsync(CreateWarehouseZoneDto dto)
        {
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == dto.WarehouseId);

            if (warehouse == null)
                throw new Exception("Warehouse not found.");

            if (!warehouse.IsActive)
                throw new Exception("Cannot add a zone to an inactive warehouse.");

            var zone = new Models.WarehouseZone
            {
                Name = dto.Name,
                Description = dto.Description,
                WarehouseId = dto.WarehouseId
            };

            _context.WarehouseZones.Add(zone);
            await _context.SaveChangesAsync();

            return new WarehouseZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description,
                WarehouseId = zone.WarehouseId
            };
        }

        public async Task<IEnumerable<WarehouseZoneResponseDto>> GetZonesByWarehouseIdAsync(int warehouseId)
        {
            return await _context.WarehouseZones
                .AsNoTracking()
                .Where(z => z.WarehouseId == warehouseId)
                .Select(z => new WarehouseZoneResponseDto
                {
                    Id = z.Id,
                    Name = z.Name,
                    Description = z.Description,
                    WarehouseId = z.WarehouseId
                })
                .ToListAsync();
        }

        public async Task<WarehouseZoneResponseDto?> GetZoneByIdAsync(int id)
        {
            return await _context.WarehouseZones
                .AsNoTracking()
                .Where(z => z.Id == id)
                .Select(z => new WarehouseZoneResponseDto
                {
                    Id = z.Id,
                    Name = z.Name,
                    Description = z.Description,
                    WarehouseId = z.WarehouseId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WarehouseZoneResponseDto?> UpdateZoneAsync(int id, UpdateWarehouseZoneDto dto)
        {
            var zone = await _context.WarehouseZones
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null)
                return null;

            zone.Name = dto.Name;
            zone.Description = dto.Description;

            await _context.SaveChangesAsync();

            return new WarehouseZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Description = zone.Description,
                WarehouseId = zone.WarehouseId
            };
        }

        public async Task<bool> DeleteZoneAsync(int id)
        {
            var zone = await _context.WarehouseZones
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null)
                return false;

            bool hasLocations = await _context.WarehouseLocations
                .AnyAsync(l => l.ZoneId == id);

            if (hasLocations)
                throw new BusinessExceptions("Cannot delete a zone that contains locations.");

            _context.WarehouseZones.Remove(zone);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
