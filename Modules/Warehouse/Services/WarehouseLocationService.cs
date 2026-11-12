using InventoryManagementAPI.Data;
using InventoryManagementAPI.Exceptions;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Locations;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Modules.Warehouse.Services
{
    public class WarehouseLocationService
    {
        private readonly InventoryManagementDbContext _context;

        public WarehouseLocationService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseLocationResponseDto> CreateLocationAsync(CreateWarehouseLocationDto dto)
        {
            var zone = await _context.WarehouseZones
                .Include(z => z.Warehouse)
                .FirstOrDefaultAsync(z => z.Id == dto.ZoneId);

            if (zone == null)
            {
                throw new Exception("Zone not found. ");
            }
            if (!zone.Warehouse.IsActive)
            {
                throw new Exception("Cannot add a location to an inactive warehouse.");
            }

            var location = new Models.WarehouseLocation
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                ZoneId = dto.ZoneId
            };

            _context.WarehouseLocations.Add(location);
            await _context.SaveChangesAsync();

            return new WarehouseLocationResponseDto
            {
                Id = location.Id,
                Code = location.Code,
                Name = location.Name,
                Description = location.Description,
                ZoneId = location.ZoneId,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt

            };
        }

        public async Task<IEnumerable<WarehouseLocationResponseDto>> GetLocationsByZoneIdAsync(int zoneId)
        {
            return await _context.WarehouseLocations
                .AsNoTracking()
                .Where(l => l.ZoneId == zoneId)
                .Select(l => new WarehouseLocationResponseDto
                {
                    Id = l.Id,
                    Code = l.Code,
                    Name = l.Name,
                    Description = l.Description,
                    ZoneId = l.ZoneId,
                    IsActive = l.IsActive,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<WarehouseLocationResponseDto?> GetLocationByIdAsync(int id)
        {
            return await _context.WarehouseLocations
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new WarehouseLocationResponseDto
                {
                    Id = l.Id,
                    Code = l.Code,
                    Name = l.Name,
                    Description = l.Description,
                    ZoneId = l.ZoneId,
                    IsActive = l.IsActive,
                    CreatedAt = l.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WarehouseLocationResponseDto?> UpdateLocationAsync(int id, UpdateWarehouseLocationDto dto)
        {
            var location = await _context.WarehouseLocations
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return null;
            }

            location.Code = dto.Code;
            location.Name = dto.Name;
            location.Description = dto.Description;

            await _context.SaveChangesAsync();

            return new WarehouseLocationResponseDto
            {
                Id = location.Id,
                Code = location.Code,
                Name = location.Name,
                Description = location.Description,
                ZoneId = location.ZoneId,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt
            };
        }

        public async Task<WarehouseLocationResponseDto?> UpdateLocationStatusAsync(int id, UpdateWarehouseLocationStatusDto dto)
        {
            var location = await _context.WarehouseLocations
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return null;
            }

            location.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return new WarehouseLocationResponseDto
            {
                Id = location.Id,
                Code = location.Code,
                Name = location.Name,
                Description = location.Description,
                ZoneId = location.ZoneId,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt
            };
        }
        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _context.WarehouseLocations
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return false;
            }
            bool hasStock = await _context.WarehouseStocks
               .AnyAsync(s => s.LocationId == id);

            bool hasTransfers = await _context.WarehouseTransfers
                .AnyAsync(t => t.SourceLocationId == id ||
                               t.DestinationLocationId == id);

            if (hasStock || hasTransfers)
                throw new BusinessExceptions(
                    "Cannot delete a location that has stock records or transfer history."
                );
            _context.WarehouseLocations.Remove(location);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
