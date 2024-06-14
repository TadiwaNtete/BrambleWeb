using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrambleWeb.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BRAMBLE_IDENTITY_CLAIMS",
                schema: "BrambleDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BRAMBLE_IDENTITY_CLAIMS", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BRAMBLE_IDENTITY_CLAIMS",
                schema: "BrambleDB");
        }
    }
}
