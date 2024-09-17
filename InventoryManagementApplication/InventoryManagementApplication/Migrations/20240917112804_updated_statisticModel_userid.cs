using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class updated_statisticModel_userid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ExecuterId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_ReporterId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_ExecuterId",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExecuterId",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "Statistics",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_ReporterId",
                table: "Statistics",
                newName: "IX_Statistics_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_AspNetUsers_UserId",
                table: "Statistics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_AspNetUsers_UserId",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Statistics",
                newName: "ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_UserId",
                table: "Statistics",
                newName: "IX_Statistics_ReporterId");

            migrationBuilder.AddColumn<string>(
                name: "ExecuterId",
                table: "Statistics",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_ExecuterId",
                table: "Statistics",
                column: "ExecuterId");

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
        }
    }
}
