using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComprasBackend.Migrations
{
    /// <inheritdoc />
    public partial class MoveComprasToSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "compras");

            migrationBuilder.RenameTable(
                name: "Purchases",
                newName: "Purchases",
                newSchema: "compras");

            migrationBuilder.RenameTable(
                name: "PurchaseItems",
                newName: "PurchaseItems",
                newSchema: "compras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Purchases",
                schema: "compras",
                newName: "Purchases");

            migrationBuilder.RenameTable(
                name: "PurchaseItems",
                schema: "compras",
                newName: "PurchaseItems");
        }
    }
}
