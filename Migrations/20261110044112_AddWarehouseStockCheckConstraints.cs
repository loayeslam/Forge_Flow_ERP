using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseStockCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_WarehouseStocks_Quantity",
                table: "WarehouseStocks",
                sql: "[Quantity] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WarehouseStocks_ReservedQuantity",
                table: "WarehouseStocks",
                sql: "[ReservedQuantity] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WarehouseStocks_ReservedQuantity_LessThan_Quantity",
                table: "WarehouseStocks",
                sql: "[ReservedQuantity] <= [Quantity]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_WarehouseStocks_Quantity",
                table: "WarehouseStocks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WarehouseStocks_ReservedQuantity",
                table: "WarehouseStocks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WarehouseStocks_ReservedQuantity_LessThan_Quantity",
                table: "WarehouseStocks");
        }
    }
}
