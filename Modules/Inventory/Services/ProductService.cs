using InventoryManagementAPI.Common.Contracts;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Exceptions;
using InventoryManagementAPI.Modules.Inventory.DTOs;
using InventoryManagementAPI.Modules.Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Modules.Inventory.Services
{
    public class ProductService
    {
        private readonly InventoryManagementDbContext _context;
        private readonly IStockAvailabilityReader _stockAvailabilityReader;
        public ProductService(InventoryManagementDbContext context, IStockAvailabilityReader stockAvailabilityReader)
        {
            _context = context;
            _stockAvailabilityReader = stockAvailabilityReader;
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts(string? search, int? categoryId)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            return await query.ToListAsync();
        }


        public async Task<Product> CreateProduct(Product product)
        {
            if (product.Price <= 0)
            {
                throw new BusinessExceptions("Price cannot be negative");
            }
            if (product.MinimumStockLevel < 0)
            {
                throw new BusinessExceptions("Minimum stock level cannot be negative");
            }

            product.CreatedAt = DateTimeOffset.UtcNow;

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;

        }

        public async Task<Product?> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return null;
            }

            if (product.Price < 0)
            {
                throw new BusinessExceptions("Price cannot be negative");
            }

            if (product.MinimumStockLevel < 0)
            {
                throw new BusinessExceptions("Minimum stock level cannot be negative");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.MinimumStockLevel = product.MinimumStockLevel;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return existingProduct;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<StockTransaction>?> GetTransactions(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            return await _context.StockTransactions
                .Where(t => t.ProductId == id)
                .ToListAsync();
        }
        public async Task<IEnumerable<LowStockProductDto>> GetLowStockProducts()
        {
            var products = await _context.Products.ToListAsync();

            var productIds = products.Select(p => p.Id).ToList();

            var availableQuantities =
                await _stockAvailabilityReader.GetAvailableQuantitiesAsync(productIds);

            var lowStockProducts = products
                .Select(product =>
                {
                    availableQuantities.TryGetValue(
                        product.Id,
                        out var availableQuantity);

                    return new LowStockProductDto
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        MinimumStockLevel = product.MinimumStockLevel,
                        AvailableQuantity = availableQuantity
                    };
                })
                .Where(p => p.AvailableQuantity <= p.MinimumStockLevel)
                .ToList();

            return lowStockProducts;
        }
    }
}
