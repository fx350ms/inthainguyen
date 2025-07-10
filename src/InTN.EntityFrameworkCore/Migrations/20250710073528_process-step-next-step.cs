using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class processstepnextstep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousStepId",
                table: "ProcessSteps");

            migrationBuilder.AddColumn<string>(
                name: "NextStepIds",
                table: "ProcessSteps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextStepIds",
                table: "ProcessSteps");

            migrationBuilder.AddColumn<int>(
                name: "PreviousStepId",
                table: "ProcessSteps",
                type: "int",
                nullable: true);
        }
    }
}
