using Microsoft.EntityFrameworkCore.Migrations;

namespace ClearSoundCompany.Migrations
{
    public partial class cartArchiveTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumPercentageForDiscount",
                table: "CartArchive",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumPriceForDiscount",
                table: "CartArchive",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumProductsQuantityForDiscount",
                table: "CartArchive",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PaintAudition",
                table: "CartArchive",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumPercentageForDiscount",
                table: "CartArchive");

            migrationBuilder.DropColumn(
                name: "MinimumPriceForDiscount",
                table: "CartArchive");

            migrationBuilder.DropColumn(
                name: "MinimumProductsQuantityForDiscount",
                table: "CartArchive");

            migrationBuilder.DropColumn(
                name: "PaintAudition",
                table: "CartArchive");
        }
    }
}
