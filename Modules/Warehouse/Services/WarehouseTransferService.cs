using InventoryManagementAPI.Data;
using InventoryManagementAPI.Exceptions;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Transfers;
using InventoryManagementAPI.Modules.Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;

namespace InventoryManagementAPI.Modules.Warehouse.Services
{
    public class WarehouseTransferService
    {
        private readonly InventoryManagementDbContext _context;

        public WarehouseTransferService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseTransferResponseDto> CreateTransferAsync(CreateWarehouseTransferDto dto)
        {
            if (dto.SourceLocationId == dto.DestinationLocationId)
                throw new BusinessExceptions("Source and Destination locations must be different. ");
            if (dto.Items == null || !dto.Items.Any())
                throw new BusinessExceptions("Transfer must contain at least one item. ");
            if (dto.Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
                throw new BusinessExceptions("Duplicate products are not allowed in the same transfer");
            if (dto.Items.Any(i => i.Quantity <= 0 || i.ProductId <= 0))
                throw new BusinessExceptions("Product ID and quantity must be greater than zero.");
            var SourceLocation = await _context.WarehouseLocations
                .Include(l => l.Zone)
                .ThenInclude(z => z.Warehouse)
                .FirstOrDefaultAsync(l => l.Id == dto.SourceLocationId);

            if (SourceLocation == null)
                throw new BusinessExceptions("Source location not found");

            var DestinationLocation = await _context.WarehouseLocations
                .Include(l => l.Zone)
                .ThenInclude(z => z.Warehouse)
                .FirstOrDefaultAsync(l => l.Id == dto.DestinationLocationId);

            if (DestinationLocation == null)
                throw new BusinessExceptions("Destination location not found");

            if (!SourceLocation.IsActive || !SourceLocation.Zone.Warehouse.IsActive)
                throw new BusinessExceptions("Source location or its warehouse is inactive.");

            if (!DestinationLocation.IsActive || !DestinationLocation.Zone.Warehouse.IsActive)
                throw new BusinessExceptions("Destination location or its warehouse is inactive.");

            var transfer = new WarehouseTransfer
            {
                SourceLocation = SourceLocation,
                DestinationLocation = DestinationLocation,
                Notes = dto.Notes,
                Status = TransferStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Items = dto.Items.Select(item => new WarehouseTransferItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
            _context.WarehouseTransfers.Add(transfer);

            await _context.SaveChangesAsync();

            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

        }
        public async Task<WarehouseTransferResponseDto> GetTransferByIdAsync(int id)
        {
            var transfer = await _context.WarehouseTransfers
           .Include(t => t.Items)
           .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                throw new BusinessExceptions("Transfer not found.");

            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
        public async Task<List<WarehouseTransferResponseDto>> GetAllTransfersAsync()
        {
            var transfers = await _context.WarehouseTransfers
                .Include(t => t.Items)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return transfers.Select(transfer => new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<WarehouseTransferResponseDto> ApproveTransferAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var transfer = await _context.WarehouseTransfers
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                throw new BusinessExceptions("Transfer not found");
            if (transfer.Status != TransferStatus.Pending)
                throw new BusinessExceptions("Only pending transfers can be approved");

            var sourceStocks = await _context.WarehouseStocks
                .Where(s => s.LocationId == transfer.SourceLocationId && s.Status == StockStatus.Available)
                .ToListAsync();

            foreach (var item in transfer.Items)
            {
                var stock = sourceStocks
                    .FirstOrDefault(s => s.ProductId == item.ProductId);

                if (stock == null)
                    throw new BusinessExceptions("Product not found in source location.");

                int available = stock.Quantity - stock.ReservedQuantity;

                if (available < item.Quantity)
                    throw new BusinessExceptions("Insufficient available stock.");
            }

            foreach (var item in transfer.Items)
            {
                var stock = sourceStocks
                    .First(s => s.ProductId == item.ProductId);

                stock.ReservedQuantity += item.Quantity;
            }

            transfer.Status = TransferStatus.Approved;
            transfer.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

        }
        public async Task<WarehouseTransferResponseDto> DispatchTransferAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var transfer = await _context.WarehouseTransfers
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                throw new BusinessExceptions("Transfer not found.");

            if (transfer.Status != TransferStatus.Approved)
                throw new BusinessExceptions("Only approved transfers can be dispatched.");

            var sourceStocks = await _context.WarehouseStocks
            .Where(s => s.LocationId == transfer.SourceLocationId && s.Status == StockStatus.Available)
            .ToListAsync();

            foreach (var item in transfer.Items)
            {
                var stock = sourceStocks
                    .FirstOrDefault(s => s.ProductId == item.ProductId);

                if (stock == null)
                    throw new BusinessExceptions("Product stock not found in source location.");

                if (stock.ReservedQuantity < item.Quantity)
                    throw new BusinessExceptions("Reserved quantity is insufficient.");
            }
            foreach (var item in transfer.Items)
            {
                var stock = sourceStocks
                    .First(s => s.ProductId == item.ProductId);

                int quantityBefore = stock.Quantity;

                stock.Quantity -= item.Quantity;
                stock.ReservedQuantity -= item.Quantity;
                stock.UpdatedAt = DateTimeOffset.UtcNow;

                var movement = new WarehouseStockMovement
                {
                    StockId = stock.Id,
                    MovementType = MovementType.Out,
                    QuantityBefore = quantityBefore,
                    QuantityChanged = -item.Quantity,
                    QuantityAfter = stock.Quantity,
                    Reason = $"Transfer #{transfer.Id} dispatched"
                };

                _context.WarehouseStockMovements.Add(movement);
            }



            transfer.Status = TransferStatus.Dispatched;
            transfer.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();


            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

        }

        public async Task<WarehouseTransferResponseDto> ReceiveTransferAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var transfer = await _context.WarehouseTransfers
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                throw new BusinessExceptions("Tranfer not found");
            if (transfer.Status != TransferStatus.Dispatched)
                throw new BusinessExceptions("Only dispatched transfers can be received. ");

            var destinationLocation = await _context.WarehouseLocations
                .Include(l => l.Zone)
                .ThenInclude(z => z.Warehouse)
                .FirstOrDefaultAsync(l => l.Id == transfer.DestinationLocationId);

            if (destinationLocation == null)
                throw new BusinessExceptions("Destination location not found.");

            if (!destinationLocation.IsActive || !destinationLocation.Zone.Warehouse.IsActive)
                throw new BusinessExceptions("Destination location or its warehouse is inactive.");

            var destinationStocks = await _context.WarehouseStocks
                .Where(s => s.LocationId == transfer.DestinationLocationId && s.Status == StockStatus.Available)
                .ToListAsync();
            foreach (var item in transfer.Items)
            {
                var stock = destinationStocks
                    .FirstOrDefault(s => s.ProductId == item.ProductId);

                if (stock == null)
                {
                    stock = new WarehouseStock
                    {
                        ProductId = item.ProductId,
                        LocationId = transfer.DestinationLocationId,
                        Quantity = item.Quantity,
                        ReservedQuantity = 0,
                        Status = StockStatus.Available,
                        CreatedAt = DateTimeOffset.UtcNow,
                        UpdatedAt = DateTimeOffset.UtcNow
                    };

                    _context.WarehouseStocks.Add(stock);
                    destinationStocks.Add(stock);
                }
                else
                {
                    stock.Quantity += item.Quantity;
                    stock.UpdatedAt = DateTimeOffset.UtcNow;
                }

                int quantityBefore = stock.Quantity - item.Quantity;

                var movement = new WarehouseStockMovement
                {
                    Stock = stock,
                    MovementType = MovementType.In,
                    QuantityBefore = quantityBefore,
                    QuantityChanged = item.Quantity,
                    QuantityAfter = stock.Quantity,
                    Reason = $"Transfer #{transfer.Id} received"
                };

                _context.WarehouseStockMovements.Add(movement);


            }

            transfer.Status = TransferStatus.Received;
            transfer.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();


            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public async Task<WarehouseTransferResponseDto> CancelTransferAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var transfer = await _context.WarehouseTransfers
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                throw new BusinessExceptions("Transfer not found");
            if (transfer.Status == TransferStatus.Dispatched || transfer.Status == TransferStatus.Received || transfer.Status == TransferStatus.Cancelled)
                throw new BusinessExceptions("This transfer cannot be cancelled in its current status.");

            if (transfer.Status == TransferStatus.Approved)
            {
                var sourceStocks = await _context.WarehouseStocks
                    .Where(s => s.LocationId == transfer.SourceLocationId
                        && s.Status == StockStatus.Available)
                    .ToListAsync();

                foreach (var item in transfer.Items)
                {
                    var stock = sourceStocks
                        .FirstOrDefault(s => s.ProductId == item.ProductId);

                    if (stock == null || stock.ReservedQuantity < item.Quantity)
                        throw new BusinessExceptions("Reserved stock not found or insufficient.");

                    stock.ReservedQuantity -= item.Quantity;
                    stock.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
            transfer.Status = TransferStatus.Cancelled;
            transfer.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new WarehouseTransferResponseDto
            {
                Id = transfer.Id,
                SourceLocationId = transfer.SourceLocationId,
                DestinationLocationId = transfer.DestinationLocationId,
                Status = transfer.Status.ToString(),
                Notes = transfer.Notes,
                CreatedAt = transfer.CreatedAt,
                UpdatedAt = transfer.UpdatedAt,
                Items = transfer.Items.Select(item => new WarehouseTransferItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
