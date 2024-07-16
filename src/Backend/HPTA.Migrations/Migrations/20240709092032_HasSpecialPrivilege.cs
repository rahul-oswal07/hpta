using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class HasSpecialPrivilege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSpecialPrivilege",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSpecialPrivilege",
                table: "Users");
        }
    }
}
