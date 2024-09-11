using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class updated_tables_added_contrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_TypeId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_TypeId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ExecuterId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ReporterId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Products_ProductId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Storages_DestinationStorageId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Storages_InitialStorageId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ProductStock",
                table: "InventoryTracker");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "FullAccess",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Storages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Storages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Statistics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Products",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "StorageId",
                table: "InventoryTracker",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InventoryTracker",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InventoryTracker",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "ActivityLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                table: "ActivityLog",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ItemType",
                table: "ActivityLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "ActivityLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_ExecuterId",
                table: "Statistics",
                column: "ExecuterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_ReporterId",
                table: "Statistics",
                column: "ReporterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Products_ProductId",
                table: "Statistics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Storages_DestinationStorageId",
                table: "Statistics",
                column: "DestinationStorageId",
                principalTable: "Storages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Storages_InitialStorageId",
                table: "Statistics",
                column: "InitialStorageId",
                principalTable: "Storages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Products_TypeId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Storages_TypeId",
                table: "ActivityLog");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ExecuterId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ReporterId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Products_ProductId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Storages_DestinationStorageId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Storages_InitialStorageId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InventoryTracker");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Storages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Storages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StorageId",
                table: "InventoryTracker",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InventoryTracker",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductStock",
                table: "InventoryTracker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FullAccess",
                table: "AspNetRoles",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "ActivityLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                table: "ActivityLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemType",
                table: "ActivityLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "ActivityLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Products_TypeId",
                table: "ActivityLog",
                column: "TypeId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Storages_TypeId",
                table: "ActivityLog",
                column: "TypeId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTracker_Products_ProductId",
                table: "InventoryTracker",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_ExecuterId",
                table: "Statistics",
                column: "ExecuterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_ReporterId",
                table: "Statistics",
                column: "ReporterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Products_ProductId",
                table: "Statistics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Storages_DestinationStorageId",
                table: "Statistics",
                column: "DestinationStorageId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Storages_InitialStorageId",
                table: "Statistics",
                column: "InitialStorageId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
