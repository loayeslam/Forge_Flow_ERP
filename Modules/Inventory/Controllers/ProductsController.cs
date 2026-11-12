using Microsoft.AspNetCore.Mvc;
using InventoryManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using InventoryManagementAPI.Modules.Inventory.Models;
using InventoryManagementAPI.Modules.Inventory.DTOs;
using InventoryManagementAPI.Modules.Inventory.Services;

namespace InventoryManagementAPI.Modules.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts(string? search, int? categoryId)
        {
           return await _productService.GetProducts(search, categoryId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Product is empty");
            }
           
                var newProduct = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    MinimumStockLevel = product.MinimumStockLevel,
                    CategoryId = product.CategoryId
                };

                var createdProduct = await _productService.CreateProduct(newProduct);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            
            
        }
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto product)
            {
               
                var newUpdatedProduct = new Product
                {
                    Id = id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    MinimumStockLevel = product.MinimumStockLevel,
                    CategoryId = product.CategoryId
                };
                
                   
                    var upadatedProduct = await _productService.UpdateProduct(id, newUpdatedProduct);
                    if (upadatedProduct == null)
                    {
                        return NotFound();
                    }
                    return NoContent();


            }
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var deleted = await _productService.DeleteProduct(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();

            }
        [HttpPost("{id}/stock/in")]
        public async Task<IActionResult> StockIn(int id, StockTransactionDto transaction)
        {
           
                var newTransaction = new StockTransaction
                {
                    Quantity = transaction.Quantity,
                    Notes = transaction.Notes
                };
                var result = await _productService.StockIn(id, newTransaction);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();

           

        }

        [HttpPost("{id}/stock/out")]
        public async Task<IActionResult> StockOut(int id, StockTransactionDto transaction)
        {
            
                var newTransaction = new StockTransaction
                {
                    Quantity = transaction.Quantity,
                    Notes = transaction.Notes
                };
                var result = await _productService.StockOut(id, newTransaction);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
           
        }

       
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<StockTransaction>>> GetTransactions(int id)
        {
            var transactions = await _productService.GetTransactions(id);

            if (transactions == null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts()
        {
            var products = await _productService.GetLowStockProducts();

            return Ok(products);
        }




    }
}
