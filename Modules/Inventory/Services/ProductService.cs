using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Exceptions;
using InventoryManagementAPI.Modules.Inventory.Models;

namespace InventoryManagementAPI.Modules.Inventory.Services
{
    public class ProductService
    {
        private readonly InventoryManagementDbContext _context;
        public ProductService(InventoryManagementDbContext context)
        {
            _context = context;
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
            product.Quantity = 0;
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

        public async Task<bool> StockIn(int id, StockTransaction transaction)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            if (transaction.Quantity <= 0)
            {
                throw new BusinessExceptions("Quantity must be greater than zero");
            }

            product.Quantity += transaction.Quantity;

            transaction.ProductId = product.Id;
            transaction.Type = StockTransactionType.StockIn;
            transaction.CreatedAt = DateTimeOffset.UtcNow;

            _context.StockTransactions.Add(transaction);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> StockOut(int id, StockTransaction transaction)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            if (transaction.Quantity <= 0)
            {
                throw new BusinessExceptions("Quantity must be greater than zero");
            }

            if (transaction.Quantity > product.Quantity)
            {
                throw new BusinessExceptions("Insufficient stock");
            }

            product.Quantity -= transaction.Quantity;

            transaction.ProductId = product.Id;
            transaction.Type = StockTransactionType.StockOut;
            transaction.CreatedAt = DateTimeOffset.UtcNow;

            _context.StockTransactions.Add(transaction);

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
        public async Task<IEnumerable<Product>> GetLowStockProducts()
        {
            return await _context.Products
                .Where(p => p.Quantity <= p.MinimumStockLevel)
                .ToListAsync();
        }
    }
}
