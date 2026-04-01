using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InParadise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update3BookingTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authority",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGateway",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authority",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentGateway",
                table: "Bookings");
        }
    }
}
