using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class removeparentnote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_ProductNotes_ParentId",
                table: "ProductNotes");

            migrationBuilder.DropIndex(
                name: "IX_ProductNotes_ParentId",
                table: "ProductNotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_ParentId",
                table: "ProductNotes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_ProductNotes_ParentId",
                table: "ProductNotes",
                column: "ParentId",
                principalTable: "ProductNotes",
                principalColumn: "Id");
        }
    }
}
