using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class orderdetailupdatefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "OrderDetails",
                newName: "TotalProductPrice");

            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "OrderDetails",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "OrderDetails",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "OrderDetails",
                newName: "NoteIds");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "TotalProductPrice",
                table: "OrderDetails",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "OrderDetails",
                newName: "ServiceName");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "OrderDetails",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NoteIds",
                table: "OrderDetails",
                newName: "Category");

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
