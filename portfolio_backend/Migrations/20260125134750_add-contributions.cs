using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio_backend.Migrations
{
    /// <inheritdoc />
    public partial class addcontributions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contributions",
                columns: table => new
                {
                    time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => x.time);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributions");
        }
    }
}
