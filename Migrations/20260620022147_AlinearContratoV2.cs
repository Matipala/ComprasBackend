using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComprasBackend.Migrations
{
    /// <inheritdoc />
    public partial class AlinearContratoV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlmacenId",
                schema: "compras",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                schema: "compras",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "compras",
                table: "PurchaseItems");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                schema: "compras",
                table: "Purchases",
                newName: "WarehouseCen");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                schema: "compras",
                table: "Purchases",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCen",
                schema: "compras",
                table: "Purchases",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductCen",
                schema: "compras",
                table: "PurchaseItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "compras",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    cen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "compras",
                table: "Suppliers",
                columns: new[] { "id", "cen", "is_active", "name" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "prov-001", true, "Distribuidora Nacional S.A." },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "prov-002", true, "Importadora Del Sur Ltda." },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "prov-003", true, "Mayorista El Buen Precio" },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "prov-004", true, "Productos Frescos del Campo" },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), "prov-005", true, "Bebidas y Licores Premium" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_cen",
                schema: "compras",
                table: "Suppliers",
                column: "cen",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "compras");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                schema: "compras",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "SupplierCen",
                schema: "compras",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ProductCen",
                schema: "compras",
                table: "PurchaseItems");

            migrationBuilder.RenameColumn(
                name: "WarehouseCen",
                schema: "compras",
                table: "Purchases",
                newName: "Supplier");

            migrationBuilder.AddColumn<Guid>(
                name: "AlmacenId",
                schema: "compras",
                table: "PurchaseItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                schema: "compras",
                table: "PurchaseItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "compras",
                table: "PurchaseItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
