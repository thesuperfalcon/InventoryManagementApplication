using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeparateProductAndStorageKeysInActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_TypeId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_TypeId",
                table: "ActivityLog");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ActivityLog",
                newName: "StorageId");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "ActivityLog",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLog_TypeId",
                table: "ActivityLog",
                newName: "IX_ActivityLog_StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ProductId",
                table: "ActivityLog",
                column: "ProductId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_ProductId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_StorageId",
                table: "ActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLog_ProductId",
                table: "ActivityLog");

            migrationBuilder.RenameColumn(
                name: "StorageId",
                table: "ActivityLog",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ActivityLog",
                newName: "ItemType");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityLog_StorageId",
                table: "ActivityLog",
                newName: "IX_ActivityLog_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Products_TypeId",
                table: "ActivityLog",
                column: "TypeId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Storages_TypeId",
                table: "ActivityLog",
                column: "TypeId",
                principalTable: "Storages",
                principalColumn: "Id");
        }
    }
}
