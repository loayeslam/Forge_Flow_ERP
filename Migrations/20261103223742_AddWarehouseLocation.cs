using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseZones_Warehouses_WarehouseID",
                table: "WarehouseZones");

            migrationBuilder.RenameColumn(
                name: "WarehouseID",
                table: "WarehouseZones",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseZones_WarehouseID",
                table: "WarehouseZones",
                newName: "IX_WarehouseZones_WarehouseId");

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseLocations_WarehouseZones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "WarehouseZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_ZoneId",
                table: "WarehouseLocations",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseZones_Warehouses_WarehouseId",
                table: "WarehouseZones",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseZones_Warehouses_WarehouseId",
                table: "WarehouseZones");

            migrationBuilder.DropTable(
                name: "WarehouseLocations");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "WarehouseZones",
                newName: "WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseZones_WarehouseId",
                table: "WarehouseZones",
                newName: "IX_WarehouseZones_WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseZones_Warehouses_WarehouseID",
                table: "WarehouseZones",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
