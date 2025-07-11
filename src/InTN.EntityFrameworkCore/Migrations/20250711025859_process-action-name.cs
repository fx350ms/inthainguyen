using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class processactionname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "ProcessSteps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "ProcessSteps");
        }
    }
}
