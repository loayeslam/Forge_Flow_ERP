using InventoryManagementAPI.Data;
using InventoryManagementAPI.Exceptions;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Stock;
using InventoryManagementAPI.Modules.Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace InventoryManagementAPI.Modules.Warehouse.Services
{
    public class WarehouseStockService
    {
        private readonly InventoryManagementDbContext _context;
        public WarehouseStockService(InventoryManagementDbContext context) {

            _context = context;
        }
        public async Task<WarehouseStockResponseDto> AddStockAsync(AddWarehouseStockDto dto)
        {
            var location = await _context.WarehouseLocations
                .Include(l => l.Zone)
                .ThenInclude(z => z.Warehouse)
                .FirstOrDefaultAsync(l => l.Id == dto.LocationId);

            if (location == null)
                throw new Exception("Location Not Found. ");

            if (!location.IsActive || !location.Zone.Warehouse.IsActive)
                throw new Exception("Cannot add stock to an inactive location or warehouse");


            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(s =>
                s.ProductId == dto.ProductId && s.LocationId == dto.LocationId && s.Status == dto.Status);

            int quantityBefore = 0;


            if (stock == null)
            {
                stock = new WarehouseStock
                {
                    ProductId = dto.ProductId,
                    LocationId = dto.LocationId,
                    Quantity = dto.Quantity,
                    Status = dto.Status
                };

                _context.WarehouseStocks.Add(stock);

            }
            else
            {
                quantityBefore = stock.Quantity;
                stock.Quantity += dto.Quantity;
                stock.UpdatedAt = DateTimeOffset.UtcNow;
            }

            var movement = new WarehouseStockMovement
            {
                Stock = stock,
                MovementType = MovementType.In,
                QuantityBefore = quantityBefore,
                QuantityChanged = dto.Quantity,
                QuantityAfter = stock.Quantity,
                Reason = dto.Reason

            };
            _context.WarehouseStockMovements.Add(movement);


            await _context.SaveChangesAsync();


            return new WarehouseStockResponseDto
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                LocationId = stock.LocationId,
                Quantity = stock.Quantity,
                ReservedQuantity = stock.ReservedQuantity,
                Status = stock.Status,
                CreatedAt = stock.CreatedAt,
                UpdatedAt = stock.UpdatedAt
            };


        }

        public async Task<List<WarehouseStockResponseDto>> GetStockByLocationAsync(int locationId)
        {
            var locationExits = await _context.WarehouseLocations
                .AnyAsync(l => l.Id == locationId);

            if (!locationExits)
                throw new Exception("Location not found ");

            var stocks = await _context.WarehouseStocks
                .Where(s => s.LocationId == locationId)
                .Select(s => new WarehouseStockResponseDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    LocationId = s.LocationId,
                    Quantity = s.Quantity,
                    ReservedQuantity = s.ReservedQuantity,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return stocks;
        }

        public async Task<List<WarehouseStockResponseDto>> GetStockByProductAsync(int productId)
        {
            var stocks = await _context.WarehouseStocks
                .Where(s => s.ProductId == productId)
                .Select(s => new WarehouseStockResponseDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    LocationId = s.LocationId,
                    Quantity = s.Quantity,
                    ReservedQuantity = s.ReservedQuantity,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return stocks;
        }

        public async Task<List<WarehouseStockResponseDto>> GetStockByWarehouseAsync(int warehouseId)
        {
            var warehouseExits = await _context.Warehouses
                .AnyAsync(w => w.Id == warehouseId);

            if (!warehouseExits)
                throw new BusinessExceptions("Warehouse not found");

            var stocks = await _context.WarehouseStocks
                .Where(s => s.Location.Zone.WarehouseId == warehouseId)
                .Select(s => new WarehouseStockResponseDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    LocationId = s.LocationId,
                    Quantity = s.Quantity,
                    ReservedQuantity = s.ReservedQuantity,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return stocks;
        }
        public async Task<WarehouseStockResponseDto> IssueStockAsync(int stockId, IssueWarehouseStockDto dto)
        {
            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(s => s.Id == stockId);

            if (stock == null)
                throw new BusinessExceptions("Stock not found. ");

            var availableQuantity = stock.Quantity - stock.ReservedQuantity;

            if (dto.Quantity > availableQuantity)
                throw new BusinessExceptions("Insufficient available stock");

            if (dto.Quantity <= 0)
                throw new BusinessExceptions("Quantity must be greater than zero.");

            int quantityBefore = stock.Quantity;
            stock.Quantity -= dto.Quantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;

            var movement = new WarehouseStockMovement
            {
                StockId = stock.Id,
                MovementType = MovementType.Out,
                QuantityBefore = quantityBefore,
                QuantityChanged = -dto.Quantity,
                QuantityAfter = stock.Quantity,
                Reason = dto.Reason
            };

            _context.WarehouseStockMovements.Add(movement);


            await _context.SaveChangesAsync();



            return new WarehouseStockResponseDto
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                LocationId = stock.LocationId,
                Quantity = stock.Quantity,
                ReservedQuantity = stock.ReservedQuantity,
                Status = stock.Status,
                CreatedAt = stock.CreatedAt,
                UpdatedAt = stock.UpdatedAt

            };
        }

        public async Task<WarehouseStockResponseDto> AdjustStockAsync(int stockId, AdjustWarehouseStockDto dto)
        {
            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(s => s.Id == stockId);
            if (stock == null)
                throw new BusinessExceptions("Stock not found");

            if (dto.PhysicalQuantity < stock.ReservedQuantity)
                throw new BusinessExceptions("Physical quantity cannot be less than reserved quantity.");

            int quantityBefore = stock.Quantity;

            int quantityChanged = dto.PhysicalQuantity - quantityBefore;

            stock.Quantity = dto.PhysicalQuantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;

            var movement = new WarehouseStockMovement
            {
                StockId = stock.Id,
                MovementType = MovementType.Adjustment,
                QuantityBefore = quantityBefore,
                QuantityChanged = quantityChanged,
                QuantityAfter = stock.Quantity,
                Reason = dto.Reason
            };

            _context.WarehouseStockMovements.Add(movement);


            await _context.SaveChangesAsync();

            return new WarehouseStockResponseDto
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                LocationId = stock.LocationId,
                Quantity = stock.Quantity,
                ReservedQuantity = stock.ReservedQuantity,
                Status = stock.Status,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

        }

        public async Task<List<WarehouseStockMovementResponseDto>> GetStockMovementsAsync(int stockId)
        {
            var stockExists = await _context.WarehouseStocks
                .AnyAsync(s => s.Id == stockId);

            if (!stockExists)
                throw new BusinessExceptions("Stock not found.");

            var movements = await _context.WarehouseStockMovements
                .Where(m => m.StockId == stockId)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new WarehouseStockMovementResponseDto
                {
                    Id = m.Id,
                    StockId = m.StockId,
                    MovementType = m.MovementType,
                    QuantityBefore = m.QuantityBefore,
                    QuantityChanged = m.QuantityChanged,
                    QuantityAfter = m.QuantityAfter,
                    Reason = m.Reason,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();

            return movements;
        }
    }
}
