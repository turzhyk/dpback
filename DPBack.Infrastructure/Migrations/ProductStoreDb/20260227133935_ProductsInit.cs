using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations.ProductStoreDb
{
    /// <inheritdoc />
    public partial class ProductsInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinesscardCoating",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinesscardCoating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCardPaper",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Thickness = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCardPaper", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Businesscards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Density = table.Column<string>(type: "text", nullable: false),
                    Finish = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesscards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinesscardCoating");

            migrationBuilder.DropTable(
                name: "BusinessCardPaper");

            migrationBuilder.DropTable(
                name: "Businesscards");
        }
    }
}
