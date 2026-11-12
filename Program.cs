using InventoryManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Middlewares;
using InventoryManagementAPI.Modules.Inventory.Services;
using InventoryManagementAPI.Modules.Warehouse.Services;
namespace InventoryManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<InventoryManagementDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Inventory Module 
            builder.Services.AddScoped<ProductService>();


            // Warehouse Module
            builder.Services.AddScoped<WarehouseService>();
            builder.Services.AddScoped<WarehouseZoneService>();
            builder.Services.AddScoped<WarehouseLocationService>();
            builder.Services.AddScoped<WarehouseStockService>();
            builder.Services.AddScoped<WarehouseTransferService>();





            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
