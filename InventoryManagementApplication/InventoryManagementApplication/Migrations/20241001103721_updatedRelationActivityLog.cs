using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class updatedRelationActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_ProductId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_StorageId",
                table: "ActivityLog");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Products_ProductId",
                table: "ActivityLog",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Storages_StorageId",
                table: "ActivityLog",
                column: "StorageId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_ProductId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_StorageId",
                table: "ActivityLog");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Products_ProductId",
                table: "ActivityLog",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Storages_StorageId",
                table: "ActivityLog",
                column: "StorageId",
                principalTable: "Storages",
                principalColumn: "Id");
        }
    }
}
