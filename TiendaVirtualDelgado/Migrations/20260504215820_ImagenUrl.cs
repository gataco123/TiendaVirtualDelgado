using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaVirtualDelgado.Migrations
{
    /// <inheritdoc />
    public partial class ImagenUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "productos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "productos");
        }
    }
}
