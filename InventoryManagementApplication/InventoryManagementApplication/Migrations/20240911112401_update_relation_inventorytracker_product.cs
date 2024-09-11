using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class update_relation_inventorytracker_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
