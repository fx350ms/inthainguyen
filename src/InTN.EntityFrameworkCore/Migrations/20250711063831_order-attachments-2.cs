using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class orderattachments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachments",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "OrderAttachments");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "OrderAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "OrderAttachments");

            migrationBuilder.AddColumn<string>(
                name: "Attachments",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "OrderAttachments",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
