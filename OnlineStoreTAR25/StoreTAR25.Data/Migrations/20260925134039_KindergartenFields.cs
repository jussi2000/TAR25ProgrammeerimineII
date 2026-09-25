using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    /// <inheritdoc />
    public partial class KindergartenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildDate",
                table: "Spaceships");

            migrationBuilder.DropColumn(
                name: "Crew",
                table: "Spaceships");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Spaceships",
                newName: "TeacherName");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Spaceships",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "EnginePower",
                table: "Spaceships",
                newName: "ChildrenCount");

            migrationBuilder.RenameColumn(
                name: "Classification",
                table: "Spaceships",
                newName: "KindergartenName");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Spaceships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Spaceships");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Spaceships",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "Spaceships",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "KindergartenName",
                table: "Spaceships",
                newName: "Classification");

            migrationBuilder.RenameColumn(
                name: "ChildrenCount",
                table: "Spaceships",
                newName: "EnginePower");

            migrationBuilder.AddColumn<DateTime>(
                name: "BuildDate",
                table: "Spaceships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Crew",
                table: "Spaceships",
                type: "int",
                nullable: true);
        }
    }
}
