using InventoryManagementAPI.Data;
using InventoryManagementAPI.Modules.Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using InventoryManagementAPI.Exceptions;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Kerberos;
using InventoryManagementAPI.Modules.Warehouse.DTOs.Warehouse;

namespace InventoryManagementAPI.Modules.Warehouse.Services
{
    public class WarehouseService
    {
        private readonly InventoryManagementDbContext _context;

        public WarehouseService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Include(w => w.WarehouseZones)
                .ToListAsync();

        }

        public async Task<Models.Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Include(w => w.WarehouseZones)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Models.Warehouse> CreateAsync(CreateWarehouseDto dto)
        {
            var warehouse = new Models.Warehouse
            {
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return warehouse;

        }

        public async Task<Models.Warehouse?> UpdateAsync(int id, UpdateWarehouseDto dto)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);
            if (warehouse == null)
            {
                return null;
            }
            warehouse.Name = dto.Name;
            warehouse.Description = dto.Description;
            warehouse.Address = dto.Address;

            await _context.SaveChangesAsync();

            return warehouse;

        }

        public async Task<Models.Warehouse?> UpdateStatusAsync(int id, UpdateWarehouseStatusDto dto)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);
            if (warehouse == null)
            {
                return null;
            }
            warehouse.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();

            return warehouse;
        }


    }
}
