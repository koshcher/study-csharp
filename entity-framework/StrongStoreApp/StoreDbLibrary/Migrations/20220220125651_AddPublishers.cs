using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreDbLibrary.Migrations
{
    public partial class AddPublishers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_Publisher_PublisherId",
                table: "Apps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher");

            migrationBuilder.RenameTable(
                name: "Publisher",
                newName: "Publishers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_Publishers_PublisherId",
                table: "Apps",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_Publishers_PublisherId",
                table: "Apps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers");

            migrationBuilder.RenameTable(
                name: "Publishers",
                newName: "Publisher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_Publisher_PublisherId",
                table: "Apps",
                column: "PublisherId",
                principalTable: "Publisher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}