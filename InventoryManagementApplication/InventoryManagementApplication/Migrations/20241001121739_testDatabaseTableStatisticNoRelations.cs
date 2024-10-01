using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class testDatabaseTableStatisticNoRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_UserId",
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

            migrationBuilder.DropIndex(
                name: "IX_Statistics_DestinationStorageId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_InitialStorageId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_ProductId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_UserId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "FinishedTime",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ProductQuantity",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "OrderTime",
                table: "Statistics",
                newName: "Moved");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationStorageName",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntitialStorageName",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Statistics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Statistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatisticRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InitialStorageId = table.Column<int>(type: "int", nullable: true),
                    DestinationStorageId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticRelation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatisticRelation_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatisticRelation_Storages_DestinationStorageId",
                        column: x => x.DestinationStorageId,
                        principalTable: "Storages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatisticRelation_Storages_InitialStorageId",
                        column: x => x.InitialStorageId,
                        principalTable: "Storages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticRelation_DestinationStorageId",
                table: "StatisticRelation",
                column: "DestinationStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticRelation_InitialStorageId",
                table: "StatisticRelation",
                column: "InitialStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticRelation_ProductId",
                table: "StatisticRelation",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticRelation_UserId",
                table: "StatisticRelation",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticRelation");

            migrationBuilder.DropColumn(
                name: "DestinationStorageName",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "IntitialStorageName",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "Moved",
                table: "Statistics",
                newName: "OrderTime");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Statistics",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Statistics",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedTime",
                table: "Statistics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductQuantity",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_DestinationStorageId",
                table: "Statistics",
                column: "DestinationStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_InitialStorageId",
                table: "Statistics",
                column: "InitialStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_ProductId",
                table: "Statistics",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_UserId",
                table: "Statistics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_UserId",
                table: "Statistics",
                column: "UserId",
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
    }
}
