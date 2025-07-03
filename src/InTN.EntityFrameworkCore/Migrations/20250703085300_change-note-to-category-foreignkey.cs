using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class changenotetocategoryforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_ProductCategoryId",
                table: "ProductNotes",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductNotes_ProductCategories_ProductCategoryId",
                table: "ProductNotes",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductNotes_ProductCategories_ProductCategoryId",
                table: "ProductNotes");

            migrationBuilder.DropIndex(
                name: "IX_ProductNotes_ProductCategoryId",
                table: "ProductNotes");
        }
    }
}
