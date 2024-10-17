using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class added_userFullName_log : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "Logs");
        }
    }
}
