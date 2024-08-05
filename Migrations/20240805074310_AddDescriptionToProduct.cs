using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedback.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tensanpham",
                table: "Product",
                newName: "TenSanPham");

            migrationBuilder.RenameColumn(
                name: "soluong",
                table: "Product",
                newName: "SoLuong");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Product",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "TenSanPham",
                table: "Product",
                newName: "tensanpham");

            migrationBuilder.RenameColumn(
                name: "SoLuong",
                table: "Product",
                newName: "soluong");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Product",
                newName: "id");
        }
    }
}
