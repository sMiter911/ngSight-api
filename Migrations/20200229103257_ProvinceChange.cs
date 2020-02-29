using Microsoft.EntityFrameworkCore.Migrations;

namespace Advantage.API.Migrations
{
    public partial class ProvinceChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Province",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Customers",
                type: "text",
                nullable: true);
        }
    }
}
