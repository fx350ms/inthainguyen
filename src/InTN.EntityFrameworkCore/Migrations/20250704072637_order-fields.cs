using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class orderfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DeliveryMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCustomerPay",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalProductAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCustomerPay",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalProductAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
