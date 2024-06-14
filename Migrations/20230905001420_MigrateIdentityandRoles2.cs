using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrambleWeb.Migrations
{
    /// <inheritdoc />
    public partial class MigrateIdentityandRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "BrambleDB",
                table: "BRAMBLE_IDENTITY_CLAIMS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "BrambleDB",
                table: "BRAMBLE_IDENTITY_CLAIMS");
        }
    }
}
