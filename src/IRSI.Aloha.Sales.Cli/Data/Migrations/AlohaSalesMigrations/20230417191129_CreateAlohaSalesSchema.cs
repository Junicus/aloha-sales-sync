using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSI.Aloha.Sales.Cli.Data.Migrations.AlohaSalesMigrations
{
    /// <inheritdoc />
    public partial class CreateAlohaSalesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessDateSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessDateKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConceptId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sales = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    GrossSales = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    NetSales = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessDateSales", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessDateSales_BusinessDateKey",
                table: "BusinessDateSales",
                column: "BusinessDateKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessDateSales");
        }
    }
}
