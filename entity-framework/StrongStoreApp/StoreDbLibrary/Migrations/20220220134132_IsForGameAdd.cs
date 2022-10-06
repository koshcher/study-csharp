using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreDbLibrary.Migrations
{
    public partial class IsForGameAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForGame",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForGame",
                table: "Categories");
        }
    }
}