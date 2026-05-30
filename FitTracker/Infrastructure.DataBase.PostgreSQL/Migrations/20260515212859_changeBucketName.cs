using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeBucketName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo_Bucket",
                table: "Exercises",
                newName: "Bucket");

            migrationBuilder.RenameColumn(
                name: "Photo_Bucket",
                table: "ExerciseProposals",
                newName: "Bucket");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "WeightHistoryPoint",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2026, 5, 16),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2026, 5, 15));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bucket",
                table: "Exercises",
                newName: "Photo_Bucket");

            migrationBuilder.RenameColumn(
                name: "Bucket",
                table: "ExerciseProposals",
                newName: "Photo_Bucket");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "WeightHistoryPoint",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2026, 5, 15),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2026, 5, 16));
        }
    }
}
