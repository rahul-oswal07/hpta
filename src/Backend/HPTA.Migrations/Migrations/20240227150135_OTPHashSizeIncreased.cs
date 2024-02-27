using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class OTPHashSizeIncreased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTP",
                table: "OTPRequests");

            migrationBuilder.AddColumn<string>(
                name: "OTPHash",
                table: "OTPRequests",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTPHash",
                table: "OTPRequests");

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "OTPRequests",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);
        }
    }
}
