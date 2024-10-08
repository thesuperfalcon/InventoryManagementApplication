using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticRelation");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "StatisticRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationStorageId = table.Column<int>(type: "int", nullable: true),
                    InitialStorageId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: true),
                    FinishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false)
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
    }
}
