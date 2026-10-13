using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateKindergardens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kindergardens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChildrenCount = table.Column<int>(type: "int", nullable: true),
                    KindergartenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kindergardens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kindergardens");
        }
    }
}
