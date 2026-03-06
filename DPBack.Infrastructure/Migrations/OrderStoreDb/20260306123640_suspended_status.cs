using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations.OrderStoreDb
{
    /// <inheritdoc />
    public partial class suspended_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderNumbers",
                startValue: 10001L);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"OrderNumbers\"')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropSequence(
                name: "OrderNumbers");
        }
    }
}
