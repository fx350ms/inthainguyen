using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTN.Migrations
{
    /// <inheritdoc />
    public partial class updateprocesssteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessSteps_ProcessSteps_PreviousStepId",
                table: "ProcessSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessSteps_Processes_ProcessId",
                table: "ProcessSteps");

            migrationBuilder.DropIndex(
                name: "IX_ProcessSteps_PreviousStepId",
                table: "ProcessSteps");

            migrationBuilder.DropIndex(
                name: "IX_ProcessSteps_ProcessId",
                table: "ProcessSteps");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Processes");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "ProcessSteps",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "RoleIds",
                table: "ProcessSteps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleIds",
                table: "ProcessSteps");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ProcessSteps",
                newName: "RoleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Processes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Processes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Processes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Processes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Processes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Processes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Processes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_PreviousStepId",
                table: "ProcessSteps",
                column: "PreviousStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_ProcessId",
                table: "ProcessSteps",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessSteps_ProcessSteps_PreviousStepId",
                table: "ProcessSteps",
                column: "PreviousStepId",
                principalTable: "ProcessSteps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessSteps_Processes_ProcessId",
                table: "ProcessSteps",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
