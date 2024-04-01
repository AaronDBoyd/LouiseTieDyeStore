using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LouiseTieDyeStore.Server.Migrations
{
    public partial class Product_Sold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Products");
        }
    }
}
